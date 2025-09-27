using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Space(10f)]
    [SerializeField] PlayerModel model;
    [SerializeField] Transform stand;

    [Space(10f)]
    public Vector2 moveDirection;
    public Vector2 zoomDirection;

    //확인용. 참조 필요 없음
    [Space(10f)]
    [SerializeField] Transform interactingEvent;
    [SerializeField] DialoguePlayer dialogueP;
    [SerializeField] DialoguePlayer currentDialogue;

    PlayerView view;
    PlayerHold jointHold;
    BookController book;

    Rigidbody rigid;
    BoxCollider boxColl;
    SphereCollider sphereColl;

    RaycastHit hit;

    Vector3 currentVelocity;
    Vector3 velocityRef;
    Vector3 flipRot;

    float flipPosZ;

    bool canGameStart = false;
    bool isGameStarted = false;

    bool canFlip = false;
    bool isFlipping = false;

    //엔딩 책 덮을때 false로 바꾸기
    bool flipvisible = true;

    bool isPulling = false;

    [HideInInspector] public bool isDialogue;

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
    }

    void Update()
    {
        //立っている状態だけFlipが可能。
        if(canFlip && OnGround() && rigid.linearVelocity.y < 0.01f)
        {
            PlayerFlip();
        }

        //Flip中の動きを調整。
        if(isFlipping)
        {
            Vector3 localPos = new Vector3(model.posX, 0, 0);
            view.AdjustmentLocalPosition(localPos);

            Vector3 pos = new Vector3(transform.position.x, transform.position.y, flipPosZ);
            view.AdjustmentPosition(pos);

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


        //範囲内のNPCを参照。複数の場合は一番近いNPCを参照。
        //参照と同時に会話アイコンが表示され、対象外になったらアイコンが非表示される。
        Collider[] eventColls = Physics.OverlapSphere(transform.position, 0.5f, model.eventLayer, QueryTriggerInteraction.Collide)
            .OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToArray();
        if(eventColls.Length > 0)
        {
            if (interactingEvent != eventColls[0].transform)
            {
                dialogueP?.CanStartEvent(false);
                interactingEvent = eventColls[0].transform;
                dialogueP = interactingEvent.GetComponentInParent<DialoguePlayer>();
                dialogueP.CanStartEvent(true);
            }
        }
        else
        {
            dialogueP?.CanStartEvent(false);
            interactingEvent = null;
            dialogueP = null;
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
        float smoothTime = inputDir.magnitude > 0 ? model.accelerationTime : model.decelerationTime;
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref velocityRef, smoothTime);

        view.SetLinearVelocity(currentVelocity);
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

    /// <summary>
    /// 
    /// </summary>
    public void Action()
    {
        //会話中
        if(isDialogue)
        {
            currentDialogue.PlayEvent();
            return;
        }

        if (!model.canMove) { return; }
        if (!OnGround() || model.isTurning) { return; }

        if (interactingEvent != null)
        {
            Transform interacting = interactingEvent.parent.transform;
            currentDialogue = interacting.GetComponent<DialoguePlayer>();

            currentDialogue.ResetEvent();
            currentDialogue.PlayEvent();

            rigid.linearVelocity = Vector3.zero;
            SetCanMove(false);
            SetIsDialogue(true);
            currentVelocity = Vector3.zero;
        }
        else if(IsHit() && OnGround() && !model.isTurning && model.canJump)
        {
            view.SetPlayerAnim("IsPulling", true);
            SetHoldingInfo(true);

            jointHold.SetJoint(model.isRight, model.jointAnchorRight, rigid, hit.rigidbody);

        }
    }

    public void ActionCancel()
    {
        if (!model.canMove) { return; }
        if (model.isHolding)
        {
            SetHoldingInfo(false);
            jointHold.ResetJoint();
        }
    }

    void SetHoldingInfo(bool isActivate)
    {
        isPulling = isActivate;
        model.isTurning = isActivate;
        model.canJump = !isActivate;
        model.isHolding = isActivate;
        model.moveSpeed = isActivate ? model.holdingSpeed : model.defaultSpeed;
        view.SetPlayerAnim(isActivate ? "StartHold" : "StopHold");
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



    public void SetCanMove(bool canMove)
    {
        model.canMove = canMove;
        rigid.isKinematic = !canMove;
    }
    public void SetCanMove(bool canMove, bool isKinematic)
    {
        model.canMove = canMove;
        rigid.isKinematic = isKinematic;
    }

    public void PlayerFlipTrigger()
    {
        if (model.canMove && model.isRight && !canFlip)
        {
            canFlip = true;
            SetCanMove(false, false);
            view.SetLinearVelocity(Vector3.zero);
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
        book.Flip();

        StartCoroutine(PlayerFlipCoroutine());
    }

    IEnumerator PlayerFlipCoroutine()
    {
        flipPosZ = transform.position.z;
        flipRot = new Vector3(0, 0, 90);
        float rotValue = -92f;
        view.StandFlip(stand, rotValue, true);

        yield return new WaitForSeconds(1.25f);
        flipPosZ = 0;
        flipRot = new Vector3(0, 0, -90);
        view.StandFlip(stand, rotValue, false);

        SetPlayerVisible(flipvisible);

        yield return new WaitForSeconds(1.75f);
        isFlipping = false;
        transform.SetParent(null);

        //アニメーションができるようにする
        view.SetPlayerAnim("CanAnim", true);
    }

    public void SetPlayerVisible(bool isVisible)
    {
        view.SetPlayerVisible(stand, isVisible);
    }



    public void SetCanGameStart() => canGameStart = true;
    public void SetGameStart()
    {
        if (!isGameStarted && canGameStart)
        {
            isGameStarted = true;
            StartCoroutine(GameStartCoroutine());
        }
    }
    IEnumerator GameStartCoroutine()
    {
        yield return null;
        PlayerFlip();
    }

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
        //Gizmos.DrawWireSphere(transform.position, eventSphereRadius);
    }
}
