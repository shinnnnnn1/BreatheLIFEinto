using System.Collections;
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

    [Space(10f)]
    [SerializeField] Transform interactingEvent;
    IEventInvoker eventInvoker;

    PlayerView view;
    BookController book;

    Rigidbody rigid;
    BoxCollider boxColl;
    SphereCollider sphereColl;

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

    //bool isPulling = false;

    [HideInInspector] public bool isDialogue;

    void Start()
    {
        view = GetComponent<PlayerView>();
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
        if(canFlip && OnGround() && rigid.linearVelocity.y < 0.01f)
        {
            PlayerFlip();
        }

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

        Collider[] eventColls = Physics.OverlapSphere(transform.position, 0.5f, model.eventLayer, QueryTriggerInteraction.Collide);
        interactingEvent = eventColls.Length > 0 ? eventColls[0].transform : null;
    }

    void FixedUpdate()
    {
        if (model.canMove)
        {
            Move();
            Turn();
        }
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



    public void Jump()
    {
        if (!model.canMove) { return; }
        if (OnGround() && model.canJump)
        {
            view.Jump(model.jumpPow);
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

    public void Action()
    {
        //会話中
        if(isDialogue)
        {
            eventInvoker?.StartEvent();
            return;
        }

        if (!model.canMove) { return; }
        if (!OnGround() || model.isTurning) { return; }

        if (interactingEvent != null)
        {
            Transform interacting = interactingEvent.parent.transform;
            IEventInvoker e = interacting.GetComponent<IEventInvoker>();
            eventInvoker = e;
            eventInvoker?.ResetEvent();
            eventInvoker?.StartEvent();
        }
        else if(IsHit())
        {

        }
    }

    public void ActionCancel()
    {
        if (!model.canMove) { return; }
        if (model.isHolding)
        {

        }
    }

    bool IsHit()
    {
        Vector3 direction = model.isRight ? Vector3.right : Vector3.left;
        if (Physics.BoxCast(transform.position + model.hitBoxOffset, model.hitBoxSize / 2, direction,
            out RaycastHit hit, Quaternion.identity, model.hitBoxDistance, model.hitLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetIsDialogue(bool isDia) => isDialogue = isDia;



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
        Vector3 isHitBoxOffset = transform.position + model.hitBoxOffset;
        Vector3 isHitBoxPos = isHitBoxOffset + (model.isRight ? Vector3.right : Vector3.left) * model.hitBoxDistance;
        Vector3 isHitBoxSize = model.hitBoxSize;
        Gizmos.DrawSphere(isHitBoxOffset, 0.01f);
        Gizmos.DrawWireCube(isHitBoxPos, isHitBoxSize);

        Gizmos.color = Color.cyan;
        float eventSphereRadius = model.eventSphereRadius;
        Gizmos.DrawWireSphere(transform.position, eventSphereRadius);
    }
}
