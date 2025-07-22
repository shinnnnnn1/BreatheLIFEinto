using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;
    ConfigurableJoint joint;
    CapsuleCollider coll;

    public Vector2 inputDirection;

    [Header("Movement")]
    [SerializeField] float moveSpd;
    [SerializeField] float maxSpd;
    float defaultMaxSpd;
    float friction;

    [Space(10f)]
    [SerializeField] float jumpPow;
    [SerializeField] float gravityMultiplier;

    [Space(10f)]
    [SerializeField] bool isRight;
    [SerializeField] bool isTurning;

    [Space(10f)]
    public bool canMove = true;
    [SerializeField] bool canJump = true;

    [Space(10f)]
    [SerializeField] float interactingObject;
    [SerializeField] bool isDialogue;

    [Header("Ground")]
    [SerializeField] LayerMask gLayer;
    [SerializeField] Vector3 gSize;
    [SerializeField] Vector3 gOffset;
    [SerializeField] float gDistance;
    RaycastHit gHit;

    [Header("Dialogue")]
    [SerializeField] NPC interactingNPC;

    [Header("Hold")]
    [SerializeField] Vector3 hSize;
    [SerializeField] Vector3 hOffset;
    [SerializeField] float hDistance;
    [SerializeField] Rigidbody defaultRigid;
    IInteractable interactable;
    Collider hColl;
    RaycastHit hHit;
    [SerializeField] bool isHolding;
    bool isPulling;

    [Space(10f)]
    [SerializeField] Transform bottom;
    [SerializeField] Vector3 Respawn;
    bool isFlipping;
    bool stopRot;

    Vector3 fRot;
    float fPos;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();
        coll = GetComponent<CapsuleCollider>();
        defaultMaxSpd = maxSpd;
        //Time.timeScale = 0.5f;
    }

    void FixedUpdate()
    {
        if(canMove)
        {
            PhysicsAdjustment();
            Movement();

            anim.SetBool("OnGround", OnGround());
            anim.SetFloat("VelocityY", rigid.linearVelocity.y);
            anim.SetFloat("InputX", inputDirection.magnitude);
        }
        else if (isFlipping)
        {
            //Teraforming Transform

            //Need to Set Position.x
            transform.localPosition = new Vector3(0, 0, 0);

            transform.position = new Vector3(transform.position.x, transform.position.y, fPos);
            //Set Rotation
            float z = GameManager.Instance.book.currentBones[8].eulerAngles.z;

            if (z > 270 || z < 90)
            {
                transform.eulerAngles = GameManager.Instance.book.currentBones[8].eulerAngles + fRot;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        
    }

    void Movement()
    {
        //Player Move
        rigid.AddForce(new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpd, ForceMode.Acceleration);

        Vector3 veloc = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);

        //Set Max Velocity
        if (veloc.magnitude > maxSpd)
        {
            veloc = veloc.normalized * maxSpd;
            rigid.linearVelocity = new Vector3(veloc.x, rigid.linearVelocity.y, veloc.z);
        }

        //Move Animation
        anim.SetFloat("VelocityX", veloc.magnitude);


        //Player Turn
        if (((inputDirection.x > 0 && !isRight) || (inputDirection.x < 0 && isRight)) && !isTurning)
        {
            Flip();
        }

        //Gravity Adjustent
        if (rigid.linearVelocity.y < 0)
        {
            rigid.AddForce(Vector3.down * gravityMultiplier);
        }

        //Add Static Gravity
        if (OnGround())
        {
            rigid.AddForce(Vector3.down);
        }

        //Hold Animation
        if (isHolding)
        {
            if (!isPulling && ((inputDirection.x > 0 && !isRight) || (inputDirection.x < 0 && isRight)))
            {
                isPulling = true;
                anim.SetBool("IsPulling", true);
            }
            else if (isPulling && ((inputDirection.x < 0 && !isRight) || (inputDirection.x > 0 && isRight)))
            {
                isPulling = false;
                anim.SetBool("IsPulling", false);
            }
        }
    }

    void PhysicsAdjustment()
    {
        //Set Moving, Stopping Drag
        if(OnGround() && inputDirection.magnitude != 0 && coll.sharedMaterial.dynamicFriction > 0.01f)
        {
            friction = Mathf.Lerp(friction, 0, 0.1f);
            coll.sharedMaterial.dynamicFriction = friction;
        }
        else if(OnGround() && inputDirection.magnitude == 0 && coll.sharedMaterial.dynamicFriction < 0.49f)
        {
            friction = Mathf.Lerp(friction, 0.5f, 0.5f);
            coll.sharedMaterial.dynamicFriction = friction;
        }

        //Set Air Drag
        Vector3 veloc = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
        if (!OnGround() && inputDirection.magnitude == 0 && veloc.magnitude > 0)
        {
            rigid.linearVelocity = Vector3.Lerp(rigid.linearVelocity, new Vector3(0, rigid.linearVelocity.y, 0), 0.1f);
            friction = Mathf.Lerp(friction, 0.5f, 0.5f);
            coll.sharedMaterial.dynamicFriction = friction;
        }
    }

    void Flip()
    {
        isTurning = true;
        isRight = !isRight;
        bottom.DOLocalRotate(new Vector3(0, isRight ? -180 : 180, 0), 0.1f)
            .SetEase(Ease.Linear).SetRelative().OnComplete(() => isTurning = false);
    }

    void Jump()
    {
        if(OnGround() && canJump && canMove)
        {
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
            rigid.AddForce(Vector3.up * jumpPow, ForceMode.Impulse);
            anim.SetTrigger("JumpTrigger");
        }
    }
    bool OnGround()
    {
        if (Physics.BoxCast(transform.position + gOffset, gSize / 2, Vector3.down, 
            out gHit, Quaternion.identity, gDistance, gLayer)) { return true; }
        else { return false; }
    }

    void Action(bool isActivate)
    {
        //Dialogue
        if (interactingNPC != null)
        {

        }
        //Interact
        else
        {

            if (isActivate && OnGround() && OnAttach() && !isTurning && canJump)
            {
                isTurning = true;
                canJump = false;
                anim.SetBool("IsPulling", true);
                anim.SetTrigger("StartHold");
                isHolding = true;
                isPulling = true;
                maxSpd = 0.3f;

                joint.anchor = isRight ? new Vector3(0.25f, 0.1f, 0) : new Vector3(-0.25f, 0.1f, 0);
                joint.axis = isRight ? Vector3.right : Vector3.left;
                joint.connectedBody = hHit.rigidbody;

                interactable = hHit.collider.GetComponent<IInteractable>();
                interactable?.OnActivate();


            }
            else if(!isActivate && isHolding)
            {
                isTurning = false;
                canJump = true;
                anim.SetTrigger("StopHold");
                isHolding = false;
                isPulling = false;
                maxSpd = defaultMaxSpd;

                interactable?.OnDeactivate();

                joint.connectedBody = defaultRigid;
            }
        }
    }

    public void JoindAdjustment(bool reset)
    {
        joint.connectedBody = reset ? defaultRigid : hHit.rigidbody;
    }

    bool OnAttach()
    {
        if (Physics.BoxCast(transform.position + Vector3.up * 0.5f + new Vector3(0, hOffset.y, 0)
            , hSize / 2, isRight ? Vector3.right : Vector3.left, out hHit, Quaternion.identity, hDistance, 1<<7)) { return true; }
        else { return false; }
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint cp in collision.contacts)
        {
            Vector3 contact = cp.point;
            Vector3 normal = cp.normal;

            Debug.DrawRay(contact, normal, Color.yellow);
        }
    }

    public void PlayerStop()
    {
        canMove = false;
        rigid.isKinematic = true;
    }

    public void PlayerFlip()
    {;
        fPos = transform.position.z;
        anim.SetBool("CanAnim", false);
        anim.SetTrigger("Stop");
        anim.SetFloat("VelocityX", 0);
        PlayerStop();
        transform.SetParent(GameManager.Instance.book.currentBones[8]);
        isFlipping = true;

        StartCoroutine(FlipAnim());
    }

    IEnumerator FlipAnim()
    {
        bottom.DOLocalRotate(new Vector3(-92, 0, 0), 1.0f).SetRelative().SetDelay(0f);
        fRot = new Vector3(0, 0, 90);

        yield return new WaitForSeconds(1.25f);

        bottom.DOLocalRotate(new Vector3(-92, 0, 0), 1.0f).SetRelative().SetDelay(0.3f);
        fRot = new Vector3(0, 0, -90);
        fPos = 0;

        yield return new WaitForSeconds(1.74f);

        //Stop Player Flip and Reset Parent
        isFlipping = false;
        transform.SetParent(null);

        //Set Player Transform
        transform.position = Respawn;
        transform.rotation = Quaternion.identity;
        anim.SetBool("CanAnim", true);

        yield return null;

        canMove = true;
        rigid.isKinematic = false;

    }



    #region INPUT
    public void InputMove(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }
    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Jump");
            Jump();
        }
    }
    public void InputAction(InputAction.CallbackContext context)
    {
        if (!canMove) { return; }

        if (context.performed)
        {
            GameManager.Instance.book.Flip();
            //Action(true);
        }
        else if(context.canceled)
        {
            //Action(false);
        }
    }
    public void InputSwitch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("switch");
            GameManager.Instance.Switch(true);

        }
    }
    public void InputPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Pause");

        }
    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.75f, new Vector3(0.75f, 1.5f, 0.05f));
        Gizmos.DrawRay(transform.position, new Vector3(inputDirection.x, 0, inputDirection.y));

        Gizmos.color = OnGround() ? Color.cyan : Color.red;
        Gizmos.DrawCube(transform.position + gOffset + Vector3.down * gDistance, gSize);

        if(!isHolding)
        {
            Gizmos.color = OnAttach() ? Color.cyan : Color.red;
            Gizmos.DrawCube(transform.position + Vector3.up * 0.5f
                    + new Vector3(isRight ? -hOffset.x : hOffset.x, hOffset.y, hOffset.z), hSize);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position + joint.anchor * 1.5f, 0.1f);
        }
    }

}
