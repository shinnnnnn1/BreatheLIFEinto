using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Space(10f)]
    public PlayerModel model;
    [SerializeField] Transform stand;

    [Space(10f)]
    public Vector2 moveDirection;
    public Vector2 zoomDirection;

    [Space(10f)]
    [SerializeField] Transform interactingEvent;
    public DialoguePlayer dialogueP;
    public DialoguePlayer currentDialogue;

    IEventInvoker iEvent;
    IEventInvoker currentEvent;

    [HideInInspector] public PlayerView view;
    PlayerHold jointHold;
    BookController book;

    [HideInInspector] public Rigidbody rigid;
    BoxCollider boxColl;
    SphereCollider sphereColl;

    RaycastHit hit;
    IPullable pullable;

    [SerializeField] Vector3 currentVelocity;
    Vector3 velocityRef;
    Vector3 flipRot;

    float flipPosZ;

    bool canGameStart = false;
    bool isGameStarted = false;

    bool canFlip = false;
    bool isFlipping = false;

    bool isLocked = false;
    [SerializeField] Vector3 lockedPos;

    //엔딩 책 덮을때 false로 바꾸기
    bool flipvisible = true;

    Vector3 tension;
    [SerializeField] float angleAccuracy;

    bool isPulling = false;

    public bool isDialogue;


    Vector3 flipPos;

    public Transform closeBone;
    public int closeIndex;

    bool lockRot;

    void Start()
    {
        view = GetComponent<PlayerView>();
        jointHold = GetComponent<PlayerHold>();
        book = FindAnyObjectByType<BookController>();   

        rigid = GetComponent<Rigidbody>();
        boxColl = GetComponent<BoxCollider>();
        sphereColl = GetComponent<SphereCollider>();

        boxColl.center = model.boxOffset;
        boxColl.size = model.boxSize;

        sphereColl.center = model.sphereOffset;
        sphereColl.radius = model.sphereRadius;

        model.isRight = true;
        model.isTurning = false;
        model.canMove = false;

        SetPlayerVisible(false);

        //本がない場合はテスト用
        if(book == null)
        {
            SetCanMove(true);
            SetPlayerVisible(true);

        }
    }

    void Update()
    {
        if(isLocked)
        {
            transform.localPosition = lockedPos;
            transform.rotation = Quaternion.identity;
        }

        //立っている状態だけFlipが可能。
        if(canFlip && OnGround() && rigid.linearVelocity.y < 0.01f)
        {
            PlayerFlip();
        }

        //Flip中の動きを調整。
        if(isFlipping)
        {
            view.AdjustmentLocalPosition(flipPos);

            if (!lockRot)
            {
                float z = book.currentBones[8].eulerAngles.z;
                if (z > 270 || z < 90)
                {
                    Vector3 rot = book.currentBones[8].eulerAngles + flipRot;
                    view.AdjustmentEulerAngles(rot);
                }
                else
                {
                    view.AdjustmentEulerAngles(Vector3.zero);
                }
            }
            
        }

        GetDialogueReference2();
    }

    /// <summary>
    /// 範囲内のDialoguePlayerを参照。複数の場合は一番近いものを参照。
    /// </summary>
    void GetDialogueReference()
    {
        //動けない状態ならreturn
        if (!model.canMove) { return; }

        //範囲内のオブジェクトを取得
        Collider[] eventColls = Physics.OverlapSphere(transform.position, model.eventSphereRadius, model.eventLayer, QueryTriggerInteraction.Collide)
            .OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToArray();

        //範囲内にオブジェクトがあった場合
        if (eventColls.Length > 0)
        {
            //現在のオブジェクトが一番近い([0])のオブジェクトじゃない場合
            if (interactingEvent != eventColls[0].transform)
            {

                //現在のオブジェクトがある場合、会話可能イメージを非表示させる
                dialogueP?.CanStartEvent(false);

                //新しいオブジェクトを参照
                interactingEvent = eventColls[0].transform;
                dialogueP = interactingEvent.GetComponentInParent<DialoguePlayer>();

                //新しいオブジェクトの会話可能イメージを表示する
                dialogueP.CanStartEvent(true);
            }
        }
        //範囲内にオブジェクトがないけどオブジェクトが参照されている場合
        else if (eventColls.Length == 0 && interactingEvent != null)
        {
            //参照されているオブジェクトの会話可能イメージを非表示させる
            dialogueP.CanStartEvent(false);

            //参照状態の初期化
            interactingEvent = null;
            dialogueP = null;
        }
    }

    /// <summary>
    /// 範囲内のDialoguePlayerを参照。複数の場合は一番近いものを参照。
    /// </summary>
    void GetDialogueReference2()
    {
        //動けない状態ならreturn
        if (!model.canMove) { return; }

        //範囲内のオブジェクトを取得
        Collider[] eventColls = Physics.OverlapSphere(transform.position, model.eventSphereRadius, model.eventLayer, QueryTriggerInteraction.Collide)
            .OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToArray();

        //範囲内にオブジェクトがあった場合
        if (eventColls.Length > 0)
        {
            //現在のオブジェクトが一番近い([0])のオブジェクトじゃない場合
            if (interactingEvent != eventColls[0].transform)
            {

                //現在のオブジェクトがある場合、会話可能イメージを非表示させる
                iEvent?.OnEventEnter(false);

                //新しいオブジェクトを参照
                interactingEvent = eventColls[0].transform;
                iEvent = interactingEvent.GetComponentInParent<IEventInvoker>();

                //新しいオブジェクトの会話可能イメージを表示する
                iEvent.OnEventEnter(true);
            }
        }
        //範囲内にオブジェクトがないけどオブジェクトが参照されている場合
        else if (eventColls.Length == 0 && interactingEvent != null)
        {
            //参照されているオブジェクトの会話可能イメージを非表示させる
            iEvent.OnEventEnter(false);

            //参照状態の初期化
            interactingEvent = null;
            iEvent = null;
        }
    }

    void FixedUpdate()
    {
        if (model.canMove)
        {
            Move();
            Turn();

            if (model.isHolding) { SetHoldingDirection(); }
        }

        ////アニメーションを手動で操作する場合がある。
        if (!model.canAnim) { return; }

        //
        view.SetPlayerAnim("OnGround", OnGround());
        //
        view.SetPlayerAnim("VelocityY", rigid.linearVelocity.y);
        //
        Vector3 veloc = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
        float velocX = Mathf.InverseLerp(0, model.moveSpeed, veloc.magnitude);
        view.SetPlayerAnim("VelocityX", velocX);
    }

    void Move()
    {
        Vector3 inputDir = new Vector3(moveDirection.x, 0, moveDirection.y).normalized;
        Vector3 targetVelocity = inputDir * model.moveSpeed;
        if (!model.isHolding)
        {
            float smoothTime = inputDir.magnitude > 0 ? model.accelerationTime : model.decelerationTime;
            currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref velocityRef, smoothTime);

            view.SetLinearVelocity(currentVelocity);
        }
        else
        {
            if(angleAccuracy < 1)
            {
                view.SetLinearVelocity(targetVelocity + tension);
            }
            else
            {
                view.SetLinearVelocity(Vector3.zero);
            }
        }
    }

    void Turn()
    {
        if (((moveDirection.x > 0 && !model.isRight) || (moveDirection.x < 0 && model.isRight)) && !model.isTurning)
        {
            model.isTurning = true;
            model.isRight = !model.isRight;
            view.Turn(stand, model.isRight, model.turnTime);
            Invoke("SetIsTurning", model.turnTime);
        }
    }

    void SetIsTurning() => model.isTurning = false;

    void SetHoldingDirection()
    {
        if (!isPulling && ((moveDirection.x > 0 && !model.isRight) || (moveDirection.x < 0 && model.isRight)))
        {
            isPulling = true;
            view.SetPlayerAnim("IsPulling", true);
        }
        else if (isPulling && ((moveDirection.x < 0 && !model.isRight) || (moveDirection.x > 0 && model.isRight)))
        {
            isPulling = false;
            view.SetPlayerAnim("IsPulling", false);
        }
    }

    public void Jump()
    {
        if (!model.canMove) { return; }
        if (OnGround() && model.canJump)
        {
            view.Jump(model.jumpPow);
            view.SetPlayerAnim("JumpTrigger");
        }
    }

    bool OnGround()
    {
        if (Physics.BoxCast(transform.position + model.jumpBoxOffset, model.jumpBoxSize / 2, Vector3.down,
            out RaycastHit hit, Quaternion.identity, model.jumpBoxDistance, model.groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }




    #region CHARACTER ACTION ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// アクションボタンを押したら実行
    /// </summary>
    public void Action()
    {
        //会話中にはプレイヤーが動けないからCanMove条件より先に実行
        if(isDialogue)
        {
            //会話を送ることだけを実行し、return
            currentEvent.OnEventInvoke();
            return;
        }

        //動けないなら、地面にいないなら、ジャンプできる状態じゃないなら、回転中ならreturn
        if (!model.canMove || !OnGround() || !model.canJump || model.isTurning) { return; }

        //接触中のイベントが存在するなら会話の開始
        if (interactingEvent != null)
        {
            currentEvent = iEvent;
            currentEvent.OnEventInvoke();
            SetCanMove(false);
            SetIsDialogue(true);
        }
        //接触しているイベントがなかったら物をつかむ動作の実行
        //範囲内につかめるオブジェクトがあったら実行
        else if(IsHit())
        {
            SetHoldingInfo(true);

            pullable = hit.collider.GetComponent<IPullable>();
            if (pullable != null)
            {
                //Debug.Log("asdasdsadasdsadsadasdasdsadasdsadsaddasasdasdsadasdasd");
                SetCanAnim(false);
                //pullable.OnActivate(this, model.isRight);
            }
            else
            {
                Debug.Log("asdasdsadasdsadsadasdasdsadasdsadsaddasasdasdsadasdasd");
                //바로 당기는 모션이 실행됨
                view.SetPlayerAnim("StartHold");
                view.SetPlayerAnim("IsPulling", true);
                jointHold.SetJoint(model.isRight, model.jointAnchorRight, hit.rigidbody);
            }
        }
    }

    public void ActionCancel()
    {
        if (!model.canMove) { return; }
        if (model.isHolding)
        {
            SetHoldingInfo(false);
            jointHold.ResetJoint();

            //pullable?.OnDeactivate();
            pullable = null;

            SetCanAnim(true);
            view.SetPlayerAnim("StopHold");
        }
    }

    void SetHoldingInfo(bool isActivate)
    {
        isPulling = isActivate;
        model.isTurning = isActivate;
        model.canJump = !isActivate;
        model.isHolding = isActivate;
        model.moveSpeed = isActivate ? model.holdingSpeed : model.defaultSpeed;
    }

    bool IsHit()
    {
        Vector3 direction = model.isRight ? Vector3.right : Vector3.left;
        if (Physics.BoxCast(transform.position + new Vector3(model.isRight ? model.hitBoxOffset.x : -model.hitBoxOffset.x, 
            model.hitBoxOffset.y, 0), model.hitBoxSize / 2, direction, out hit, Quaternion.identity, model.hitBoxDistance, model.hitLayer))
        {
            return true;
        }
        else { return false; }
    }

    public void SetIsDialogue(bool isDia)
    {
        isDialogue = isDia;
        if(!isDia)
        {
            currentDialogue = null;
        }
    }
    public void SetDialogueAuto(IEventInvoker e)
    {
        currentEvent = e;
        SetIsDialogue(true);
    }

    public void SetConstraints(bool dirX)
    {
        rigid.constraints = dirX ? RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation :
                RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
    }
    public void SetConstraints()
    {
        rigid.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void SetTension(Vector3 t) => tension = t;
    public void SetAngleAccuracy(float accuracy) => angleAccuracy = accuracy;



    public void TurnBook(bool isRightTurn)
    {
        if (!model.canMove || !OnGround()) { return; }

        book.TurnBook(isRightTurn, out bool canTurn);
        if (!canTurn) { return; }

        SetCanMove(false);
        LockPlayer(true);
    }

    public void LockPlayer(bool startLock)
    {
        if(startLock)
        {
            isLocked = true;
            Transform closeBone = null;
            float dis = 100;
            Transform[] t = transform.position.x > 0 ? book.rightBones : book.leftBones;
            for (int i = 0; i < t.Length; i++)
            {
                float close = Vector3.Distance(transform.position, t[i].position);
                if (close < dis)
                {
                    dis = close;
                    closeBone = t[i];
                }
            }
            transform.SetParent(closeBone);

            lockedPos = transform.localPosition;
        }
        else
        {
            isLocked = false;
            transform.SetParent(null);
            SetCanMove(true);
        }
    }
    #endregion


    #region PLAYER CONTROL ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetCanControl(bool control)
    {
        Debug.Log("PlayerSetCanControl " + control);
        model.canControl = control;
    }

    public void SetCanMove(bool move)
    {
        Debug.Log("PlayerSetCanMove " + move + "" + !move);
        model.canMove = move;
        rigid.isKinematic = !move;

        if (!move)
        {
            view.SetLinearVelocity(Vector3.zero);
            currentVelocity = Vector2.zero;
        }
    }
    public void SetCanMove(bool move, bool kinematic)
    {
        Debug.Log("PlayerSetCanMove " + move + "" + kinematic);
        model.canMove = move;
        rigid.isKinematic = kinematic;

        if (!move)
        {
            view.SetLinearVelocity(Vector3.zero);
            currentVelocity = Vector2.zero;
        }
    }
    public void SetPlayerVisible(bool isVisible)
    {
        view.SetPlayerVisible(stand, isVisible);
    }
    public void SetCanAnim(bool canAnim) => model.canAnim = canAnim;
    #endregion



    #region PLAYER FLIP ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// 進行ができる状態でFlipTriggerに触れたら実行。Flipができる状態にする
    /// </summary>
    public void PlayerFlipTrigger()
    {
        if (model.canMove && model.isRight  && moveDirection.magnitude > 0 && !canFlip)
        {
            //空中でトリガーが発動された場合も想定し、操作はできないけど物理は生きている状態にする
            SetCanMove(false, false);

            //Flipができる状態にする
            canFlip = true;
        }
    }

    void PlayerFlip()
    {
        currentVelocity = Vector3.zero;
        canFlip = false;
        isFlipping = true;
        SetCanMove(false);

        //Flip中はアニメーションしないようにする
        view.SetPlayerAnim("CanAnim", false);
        view.SetPlayerAnim("VelocityX", 0);

        transform.SetParent(book.currentBones[8]);
        flipPos = transform.localPosition;

        book.Flip(out int currentPage);

        StartCoroutine(PlayerFlipCoroutine());
    }

    IEnumerator PlayerFlipCoroutine()
    {
        flipRot = new Vector3(0, 0, 90);
        float rotValue = -92f;
        view.StandFlip(stand, rotValue, true);

        yield return new WaitForSeconds(1.25f);
        yield return new WaitForSeconds(model.respawnDelay[book.currentPage]);

        FlipReposition();

        flipRot = new Vector3(0, 0, -90);
        view.StandFlip(stand, rotValue, false);

        /*
        if (model.respawnException[book.currentPage].x > 0)
        {
            lockRot = true;
            transform.rotation = Quaternion.identity;
        }
        */

        SetPlayerVisible(flipvisible);

        yield return new WaitForSeconds(1.75f);
        yield return new WaitForSeconds(1.75f);
        isFlipping = false;
        transform.SetParent(null);

        //アニメーションができるようにする
        view.SetPlayerAnim("CanAnim", true);

        yield return new WaitForSeconds(0.5f);
        //플레이어의 부모를 초기화 한 뒤에 currentPage의 애니메이션을 초기화 시켜야함.
        book.view.PlayPageAnimation(2, "Reset");

        lockRot = false;
    }

    /// <summary>
    /// 새로운 리스폰 장소 찾기
    /// </summary>
    void FlipReposition()
    {
        Vector3 respawnPos = model.respawnException[book.currentPage];
        if (respawnPos == Vector3.zero)
        {
            //flipPos = new Vector3(flipPos.x, flipPos.y, flipPosZ);

            SetPlayerVisible(false);

            transform.SetParent(null);
            transform.position = model.defaultRespawn;

            

            transform.SetParent(book.leftBones[8]);
            flipPos = transform.localPosition;
            Debug.Log(transform.localPosition);

            transform.SetParent(book.cb2s[8]);

            SetPlayerVisible(true);
        }
        else
        {
            Transform[] newPage = respawnPos.x > 0 ? book.rightBones : book.leftBones;

            float dis = 100;
            for (int i = 0; i < newPage.Length; i++)
            {
                float close = Vector3.Distance(model.respawnException[book.currentPage], newPage[i].position);
                if (close < dis)
                {
                    dis = close;
                    closeBone = newPage[i];
                    closeIndex = i;
                }
            }

            transform.SetParent(null);
            transform.position = model.respawnException[book.currentPage];

            transform.SetParent(closeBone);
            flipPos = transform.localPosition;

            Transform[] newnewPage = respawnPos.x > 0 ? book.rightBones : book.currentBones;

            transform.SetParent(newnewPage[closeIndex]);

            
        }
    }
    #endregion



    #region GAME START ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    public void SetCanGameStart() => canGameStart = true;
    public void SetGameStart()
    {
        if (!isGameStarted && canGameStart)
        {
            isGameStarted = true;
            book.GameStart();
            StartCoroutine(GameStartCoroutine());
        }
    }
    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSeconds(3f);
        PlayerFlip();
    }

    public void Ending()
    {
        StartCoroutine(LastFlip());
    }
    IEnumerator LastFlip()
    {
        yield return new WaitForSeconds(3f);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 boxPosition = transform.position + model.boxOffset;
        Vector3 boxSize = model.boxSize;
        Gizmos.DrawWireCube(boxPosition, boxSize);

        Vector3 spherePosition = transform.position + model.sphereOffset;
        float sphereRadius = model.sphereRadius;
        Gizmos.DrawWireSphere(spherePosition, sphereRadius);

        Gizmos.color = OnGround() ? Color.cyan : Color.red;
        Vector3 onGroundBoxOffset = transform.position + model.jumpBoxOffset;
        Vector3 onGroundBoxPos = onGroundBoxOffset + Vector3.down * model.jumpBoxDistance;
        Vector3 onGroundBoxSize = model.jumpBoxSize;
        Gizmos.DrawSphere(onGroundBoxOffset, 0.01f);
        Gizmos.DrawWireCube(onGroundBoxPos, onGroundBoxSize);

        Gizmos.color = IsHit() ? Color.cyan : Color.red;
        Vector3 isHitBoxOffset = transform.position + new Vector3(model.isRight ? model.hitBoxOffset.x : -model.hitBoxOffset.x, model.hitBoxOffset.y, 0);
        Vector3 isHitBoxPos = isHitBoxOffset + (model.isRight ? Vector3.right : Vector3.left) * model.hitBoxDistance;
        Vector3 isHitBoxSize = model.hitBoxSize;
        Gizmos.DrawSphere(isHitBoxOffset, 0.01f);
        Gizmos.DrawWireCube(isHitBoxPos, isHitBoxSize);

        Gizmos.color = Color.cyan;
        float eventSphereRadius = model.eventSphereRadius;
        Gizmos.DrawWireSphere(transform.position, eventSphereRadius);
    }
}
