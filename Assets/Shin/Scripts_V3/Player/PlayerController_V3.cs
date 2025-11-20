using System.Collections;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

public class PlayerController_V3 : MonoBehaviour, IPlayerController
{
    [SerializeField] PlayerModel_V3 model;
    [SerializeField] PlayerRespawnPosition respawn;
    [SerializeField] CinemachinePositionComposer positionComposer;

    PlayerView_V3 view;

    Rigidbody rigid;
    BoxCollider boxColl;
    SphereCollider sphereColl;

    [Space(10f)]
    public Vector2 moveDirection;
    public Vector2 zoomDirection;

    IBookController bookController;

    //本をめくる時に使用
    /// <summary>　Triggerに触れ、Flipできる状態になったかを確認　</summary>
    bool canFlip = false;
    /// <summary>　Flipしているかを確認　</summary>
    bool isFlipping = false;

    //

    //本を回すときに使用

    Transform[] pageL, pageR, pageLC, pageRC;


    [SerializeField] int closeIndex;
    [SerializeField] Transform closeBone;
    [SerializeField] Transform rotBone;

    [SerializeField] Vector3 flipPos;
    [SerializeField] Vector3 flipRot;

    //엔딩때 Flip 후 Open할때 캐릭터 안보이게하는,
    //헨젤과 그레텔 처음에 나올때 안보이게 하는거
    // 즉 플립 후에 나오긴 하지만 투명한 상태. 이벤트로 다시 수동으로 나오는 모션을 만들든지 해야함
    [SerializeField] bool flipVisible;

    [Space(10f)]
    [SerializeField] Transform interacting;
    IEventInvoker interactingEvent;
    IEventInvoker currentEvent;


    bool canGameStart = false;

    [SerializeField] ConfigurableJoint joint;
    [SerializeField] Rigidbody targetRigid;

    //Zoom関連
    [SerializeField] Vector2 zoom_Min_Max;
    [SerializeField] Vector3 zoom_Current_Target_Speed;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー




    RaycastHit hit;
    IInteractable interactable;
    IPullable pullable;

    [SerializeField] Vector3 currentVelocity;
    Vector3 velocityRef;

    bool isLocked = false;
    [SerializeField] Vector3 lockedPos;

    //엔딩 책 덮을때 false로 바꾸기
    bool flipvisible = true;

    Vector3 tension;
    [SerializeField] float angleAccuracy;

    bool isPulling = false;

    public bool isDialogue;

    bool lockRot;



    void Start()
    {
        //参照
        rigid = GetComponent<Rigidbody>();
        view = GetComponent<PlayerView_V3>();
        joint = GetComponentInChildren<ConfigurableJoint>();
        bookController = GameObject.FindGameObjectWithTag("BookController").GetComponent<IBookController>();

        //コライダーを参照し、詳細を設定
        boxColl = GetComponent<BoxCollider>();
        boxColl.center = model.boxOffset;
        boxColl.size = model.boxSize;
        sphereColl = GetComponent<SphereCollider>();
        sphereColl.center = model.sphereOffset;
        sphereColl.radius = model.sphereRadius;

        //本の有無とは関係ないモデル変数の初期化
        model.isRight = true;
        model.isTurning = false;

        //本がある場合の初期化
        if (bookController != null)
        {
            SetCanMove(false);
            SetPlayerVisible(false);
        }
        //開発用。本がない場合の初期化
        else
        {
            SetCanMove(true);
            SetPlayerVisible(true);
        }
    }

    #region ●UPDATE ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void Update()
    {
        //本を回すときキャラクターを固定する
        /// <seealso cref="LockPlayer(bool)"/>
        if (isLocked)
        {
            //位置と回転を固定
            transform.localPosition = lockedPos;
            transform.rotation = Quaternion.identity;
        }

        //地面にいる状態だけFlipが可能だから地面にいるか確認し続ける
        /// <seealso cref="PlayerFlipTrigger()"/>
        if (canFlip && OnGround() && rigid.linearVelocity.y < 0.01f)
        {
            //Flipを開始
            bookController.Flip();
        }

        //Flip中の動きを調整
        FlipAdjustment();

        //範囲内のイベントを参照。複数の場合は一番近いものを参照
        GetDialogueReference();

        //範囲内のInteractableを参照。
        GetInteractableReference();

        Zoom();
    }

    /// <summary>
    /// Flip中の動きを調整
    /// </summary>
    /// <seealso cref="PlayerFlip(bool, int)"/>
    void FlipAdjustment()
    {
        if (isFlipping)
        {
            //LocalPositionを固定
            view.AdjustmentLocalPosition(flipPos);

            //回転は精度を上げるために手動で設定
            float z = rotBone.eulerAngles.z;
            //回転をする場合
            if (z > 270 || z < 90)
            {
                //近いボーンの回転に任意の数値を追加
                Vector3 rot = rotBone.eulerAngles + flipRot;
                view.AdjustmentEulerAngles(rot);
            }
            //回転を止めてもいい場合（最初と最後）
            else
            {
                view.AdjustmentEulerAngles(Vector3.zero);
            }
        }
    }

    /// <summary>
    /// 範囲内のイベントを参照。複数の場合は一番近いものを参照。
    /// </summary>
    void GetDialogueReference()
    {
        //動けない状態ならreturn
        if (!model.canMove) { return; }

        //範囲内のオブジェクトを取得
        Collider[] eventColls = Physics.OverlapSphere(transform.position, model.eventSphereRadius, model.eventLayer, 
            QueryTriggerInteraction.Collide).OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToArray();

        //範囲内にオブジェクトがあった場合
        if (eventColls.Length > 0)
        {
            //現在のオブジェクトが一番近い([0])のオブジェクトじゃない場合
            if (interacting != eventColls[0].transform)
            {
                //現在のオブジェクトがある場合、会話可能イメージを非表示させる
                interactingEvent?.OnEventEnter(false);

                //新しいオブジェクトを参照
                interacting = eventColls[0].transform;
                interactingEvent = interacting.GetComponentInParent<IEventInvoker>();

                //新しいオブジェクトの会話可能イメージを表示する
                interactingEvent.OnEventEnter(true);
            }
        }
        //範囲内にオブジェクトがないけどオブジェクトが参照されている場合
        else if (eventColls.Length == 0 && interacting != null)
        {
            //参照されているオブジェクトの会話可能イメージを非表示させる
            interactingEvent.OnEventEnter(false);

            //参照状態の初期化
            interacting = null;
            interactingEvent = null;
        }
    }

    /// <summary>
    /// 範囲内の掴むオブジェクトを参照。ハイライト表現をする
    /// </summary>
    void GetInteractableReference()
    {
        if(!model.isHolding)
        {
            if (IsHit())
            {
                interactable = hit.collider.GetComponent<IInteractable>();
                interactable.OnEnter(model.isRight);
            }
            else if (!IsHit() && interactable != null)
            {
                interactable?.OnExit();
                interactable = null;
            }
        }
    }

    void Zoom()
    {
        if (model.canMove)
        {
            zoom_Current_Target_Speed.y -= zoomDirection.y;

            zoom_Current_Target_Speed.y = 
                Mathf.Clamp(zoom_Current_Target_Speed.y, zoom_Min_Max.x, zoom_Min_Max.y);

            zoom_Current_Target_Speed.x = 
                Mathf.Lerp(zoom_Current_Target_Speed.x, zoom_Current_Target_Speed.y, Time.deltaTime * zoom_Current_Target_Speed.z);

            positionComposer.CameraDistance = zoom_Current_Target_Speed.x;
        }
    }
    #endregion

    #region FIXED UPDATE ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    void FixedUpdate()
    {
        if (model.canMove)
        {
            Move();
            Turn();

            if (model.isHolding) { SetHoldingDirection(); }
        }

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
            if (angleAccuracy < 1)
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
            view.Turn(model.isRight, model.turnTime);
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
    #endregion

    #region JUMP ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
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
    #endregion

    #region CHARACTER ACTION ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// アクションボタンを押したら実行
    /// </summary>
    /// <seealso cref="PlayerActionInput_V3.InputAction(UnityEngine.InputSystem.InputAction.CallbackContext)"/>
    public void Action()
    {
        //会話中にはプレイヤーが動けないからCanMove条件より先に実行
        if (isDialogue)
        {
            //会話を送ることだけを実行し、return
            currentEvent.OnEventInvoke();
            return;
        }

        //動けないなら、地面にいないなら、ジャンプできる状態じゃないなら、回転中ならreturn
        if (!model.canMove || !OnGround() || !model.canJump || model.isTurning) { return; }

        //接触中のイベントが存在するなら会話の開始
        if (interacting != null)
        {
            currentEvent = interactingEvent;
            currentEvent.OnEventInvoke();
            SetCanMove(false);
            SetIsDialogue(true);
        }
        //接触しているイベントがなかったら物をつかむ動作の実行
        //範囲内につかめるオブジェクトがあったら実行
        else if (IsHit())
        {
            SetHoldingInfo(true);

            pullable = hit.collider.GetComponent<IPullable>();

            //IPullableがあるなら
            if (pullable != null)
            {
                pullable.OnActivate(this, model.isRight);
            }
            //IPullableがないなら（栞など）
            else
            {
                //そのまま引っ張るモーションに切り替える
                view.SetPlayerAnim("StartHold");
                view.SetPlayerAnim("IsPulling", true);

                //Jointの設定をする
                SetJoint(model.isRight, model.jointAnchorRight, hit.rigidbody);
            }
        }
    }
    /// <summary>
    /// アクションボタンを離したら実行
    /// </summary>
    /// <seealso cref="PlayerActionInput_V3.InputAction(UnityEngine.InputSystem.InputAction.CallbackContext)"/>
    public void ActionCancel()
    {
        if (!model.canMove) { return; }
        if (model.isHolding)
        {
            SetHoldingInfo(false);
            ResetJoint();

            pullable?.OnDeactivate(model.isRight);
            pullable = null;

            //SetCanAnim(true);
            view.SetPlayerAnim("StopHold");
        }
    }

    void SetIsDialogue(bool isDia)
    {
        isDialogue = isDia;
    }

    public void PlayAutoEvent(DialogueEvent_V3 dialogue)
    {
        currentEvent = dialogue;
        currentEvent.OnEventInvoke();
        SetCanMove(false);
        SetIsDialogue(true);
    }
    public void OnDialogueEnd()
    {
        currentEvent = null;
        SetIsDialogue(false);
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

    #endregion

    #region HOLD ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    bool IsHit()
    {
        Vector3 direction = model.isRight ? Vector3.right : Vector3.left;
        if (Physics.BoxCast(transform.position + new Vector3(model.isRight ? model.hitBoxOffset.x : -model.hitBoxOffset.x,
            model.hitBoxOffset.y, 0), model.hitBoxSize / 2, direction, out hit, Quaternion.identity, model.hitBoxDistance, model.hitLayer))
        { return true; }
        else { return false; }
    }

    void SetHoldingInfo(bool isActivate)
    {
        isPulling = isActivate;
        model.isTurning = isActivate;
        model.canJump = !isActivate;
        model.isHolding = isActivate;
        model.moveSpeed = isActivate ? model.holdingSpeed : model.defaultSpeed;
    }

    public void SetJoint(bool isRight, Vector2 anchor, Rigidbody target)
    {
        targetRigid = target;

        rigid.mass = 100;
        targetRigid.mass = 1;

        joint.anchor = new Vector3(isRight ? anchor.x : -anchor.x, anchor.y, 0);
        joint.axis = isRight ? Vector3.right : Vector3.left;
        joint.connectedBody = target;
    }
    public void ResetJoint()
    {
        rigid.mass = 1;
        if (targetRigid != null)
        {
            targetRigid.mass = 100;
            targetRigid = null;
        }

        joint.connectedBody = null;
    }
    #endregion

    #region TURNBOOK ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// 本を回す操作
    /// </summary>
    /// <seealso cref="PlayerActionInput_V3.InputTurnBookL(UnityEngine.InputSystem.InputAction.CallbackContext)"/>
    /// <seealso cref="PlayerActionInput_V3.InputTurnBookR(UnityEngine.InputSystem.InputAction.CallbackContext)"/>
    public void TurnBook(bool isRightTurn)
    {
        //動けない、地面にいないなら return
        if (!model.canMove || !OnGround()) { return; }

        /*
        book.TurnBook(isRightTurn, out bool canTurn);
        if (!canTurn) { return; }

        SetCanMove(false);
        LockPlayer(true);
        */
    }
    /*
    public void LockPlayer(bool startLock)
    {
        if (startLock)
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
    */
    #endregion

    #region ZOOM ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    #endregion

    #region ●PLAYER CONTROL ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// 操作ができる状態を設定
    /// </summary>
    /// <seealso cref=""/>
    public void SetCanControl(bool control)
    {
        Debug.Log("PlayerSetCanControl " + control);

        //操作ができる状態を設定
        model.canControl = control;
    }
    /// <summary>
    /// キャラクターの動ける状態を設定
    /// </summary>
    /// <seealso cref="PlayerFlip(bool, int)"/> <seealso cref="StopFlip()"/>
    public void SetCanMove(bool move)
    {
        Debug.Log("PlayerSetCanMove " + move + "" + !move);

        //キャラクターの動ける状態を設定
        model.canMove = move;
        rigid.isKinematic = !move;

        //動けない状態にする時は速度を初期化する
        if (!move)
        {
            view.SetLinearVelocity(Vector3.zero);
            currentVelocity = Vector3.zero;
        }
    }
    /// <summary>
    /// キャラクターの動ける状態を設定。CanMoveとIsKinematicを個別に設定
    /// </summary>
    /// <seealso cref="PlayerFlipTrigger()"/> 
    public void SetCanMove(bool move, bool kinematic)
    {
        Debug.Log("PlayerSetCanMove " + move + "" + kinematic);

        //キャラクターの動ける状態を設定
        model.canMove = move;
        rigid.isKinematic = kinematic;

        //動けない状態にする時は速度を初期化する
        if (!move)
        {
            view.SetLinearVelocity(Vector3.zero);
            currentVelocity = Vector3.zero;
        }
    }
    /// <summary>
    /// キャラクターの表示状態を設定
    /// </summary>
    /// <seealso cref="PlayerFlip(bool, int)"/>
    public void SetPlayerVisible(bool isVisible) => view.SetPlayerVisible(isVisible);

    /// <summary>
    /// キャラクターのアニメーションを手動で変更
    /// </summary>
    /// <param name="trigger"></param>
    public void SetPlayerAnimation(string trigger) => view.SetPlayerAnim(trigger);

    #endregion

    #region ●PLAYER FLIP ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// 進行ができる状態でFlipTriggerに触れたら実行。Flipができる状態にする
    /// </summary>
    /// <seealso cref="FlipTrigger_V3.OnCollisionStay(Collision)"/>
    public void PlayerFlipTrigger()
    {
        if (model.canMove && moveDirection.magnitude > 0 && !canFlip)
        {
            //空中でTriggerが発動された場合も想定し、操作はできないけど物理は生きている状態にする
            SetCanMove(false, false);

            //Flipができる状態にする
            canFlip = true;
        }
    }

    /// <summary>
    /// Flip中にキャラクターを閉じ、位置を設定し、開く動作
    /// </summary>
    /// <seealso cref="BookController_V3.Flip()"/>
    public void PlayerFlip(bool isOpen, int currentPage)
    {
        //プレイヤーを閉じる false
        if (!isOpen)
        {
            //Flipを実行する設定
            SetCanMove(false);
            canFlip = false;
            isFlipping = true;
            
            //Flip中はアニメーションしないようにする
            view.SetPlayerAnim("CanAnim", false);
            view.SetPlayerAnim("Idle");

            //親の設定
            FlipReposition(isOpen, transform.position);
            
            //追加の回転を設定
            flipRot = new Vector3(0, 0, 90);

            //Standの倒れるモーション
            float rotValue = -92f;
            view.StandFlip(rotValue, false);
        }
        //プレイヤーを開く true
        else
        {
            //位置と親の設定
            FlipReposition(isOpen, respawn.position[currentPage]);

            //追加の回転を設定
            flipRot = new Vector3(0, 0, -90);

            //キャラクターを表示
            SetPlayerVisible(true);

            //Standの倒れるモーション
            float rotValue = -92f;
            view.StandFlip(rotValue, true);
        }
    }

    /// <summary>
    /// キャラクターの親を設定、開く時は位置も設定
    /// </summary>
    /// <seealso cref="PlayerFlip(bool, int)"/>
    void FlipReposition(bool isOpen, Vector3 reposition)
    {
        //次の位置を設定
        Vector3 pos = reposition == Vector3.zero ? respawn.defaultPosition : reposition;
        pos = new Vector3(pos.x, pos.y, -pos.z);

        //近いページを探す
        Transform[] newPage = isOpen ? (pos.x < 0 ? pageL : pageR) : (pos.x < 0 ? pageL : pageRC);

        //近いBone、Indexを取得
        float dis = 100;
        for (int i = 0; i < newPage.Length; i++)
        {
            float close = Vector3.Distance(pos, newPage[i].position);
            if (close < dis)
            {
                dis = close;
                closeBone = newPage[i];
                closeIndex = i;
            }
        }

        //閉じるとき
        if(!isOpen)
        {
            //親、位置、回転ボーンを設定
            transform.SetParent(closeBone);
            flipPos = transform.localPosition;
            rotBone = closeBone;
        }
        //開くとき
        else
        {
            //目標位置に移動
            transform.SetParent(null);
            transform.position = pos;

            //目標のLocalPositionを取得
            transform.SetParent(closeBone);
            flipPos = transform.localPosition;

            //新しいボーンを取得
            Transform[] newnewPage = pos.x < 0 ? pageLC : pageR;
            closeBone = newnewPage[closeIndex];

            //回転はLCでやってはいけないのでLCの場合だけRCに変更
            rotBone = newnewPage == pageLC ? pageRC[closeIndex] : pageR[closeIndex];

            //親を設定
            transform.SetParent(closeBone);
        }
    }

    /// <summary>
    /// Flipが終わり、キャラクターを初期化する
    /// </summary>
    /// <seealso cref="BookController_V3.Flip()"/>
    public void StopFlip()
    {
        isFlipping = false;
        transform.SetParent(null);
        view.SetPlayerAnim("CanAnim", true);
        //SetCanMove(true);
    }
    #endregion

    #region ●GAME START ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// 本の初期設定ができてゲームをスタートできる状態にする。ついでに本のデーターも渡す
    /// </summary>
    /// <seealso cref="BookController_V3.Start()"/>
    public void SetCanGameStart(Transform[] pL, Transform[] pR, Transform[] pLC, Transform[] pRC)
    {
        canGameStart = true;
        pageL = pL;
        pageR = pR;
        pageLC = pLC;
        pageRC = pRC;
    }

    /// <summary>
    /// どんなキーを押しても実行
    /// </summary>
    /// <seealso cref="PlayerActionInput_V3.InputAnyKey(UnityEngine.InputSystem.InputAction.CallbackContext)"/>
    public void IAnyKey()
    {
        //ゲームのスタート
        if (canGameStart)
        {
            //最初の本のページをめくる
            bookController.GameStart(true);
            canGameStart = false;
        }
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
