using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerCtrl : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;
    ConfigurableJoint joint;
    Rigidbody sampleRigid;
    Collider coll;

    public Vector2 inputDirection;

    [Header("Movement")]
    [SerializeField] float moveSpd;
    [SerializeField] float maxSpd;

    [Space(10f)]
    [SerializeField] float jumpPow;

    [Space(10f)]
    [SerializeField] bool isRight;
    [SerializeField] bool isTurning;

    [Space(10f)]
    [SerializeField] bool canMove = true;
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

    [Space(10f)] [Header("Hold")]
    [SerializeField] Rigidbody holdingObject;
    [SerializeField] Vector3 hSize;
    [SerializeField] Vector3 hOffset;
    [SerializeField] float hDistance;
    RaycastHit hHit;
    
    [Space(10f)]
    [SerializeField] Transform bottom;


    
    [SerializeField] float a;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        joint = GetComponent<ConfigurableJoint>();
        coll = GetComponent<Collider>();
        sampleRigid = holdingObject;
        //Time.timeScale = 0.1f;
    }

    void Update()
    {
        //float b = Vector3.Distance(transform.position, )
    }

    void FixedUpdate()
    {
        Stop();
        if (!canMove) { return; }
        Move();
        anim.SetFloat("Y", rigid.linearVelocity.y);

        if(canJump)
        {

        }
    }

    void Move()
    {
        rigid.AddForce(new Vector3(inputDirection.x, 0, inputDirection.y) * moveSpd, ForceMode.Acceleration);

        Vector3 veloc = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
        if (veloc.magnitude > maxSpd)
        {
            veloc = veloc.normalized * maxSpd;
            rigid.linearVelocity = new Vector3(veloc.x, rigid.linearVelocity.y, veloc.z);
        }
        anim.SetFloat("Move", veloc.magnitude);

        if(((inputDirection.x > 0 && !isRight) || (inputDirection.x < 0 && isRight)) && !isTurning)
        {
            Flip();
        }
    }
    void Stop()
    {
        if(inputDirection.x == 0 && Mathf.Abs(rigid.linearVelocity.x) > 0.001f)
        {
            float velocX = rigid.linearVelocity.x;
            velocX = Mathf.Lerp(velocX, 0, 0.1f);
            rigid.linearVelocity = new Vector3(velocX, rigid.linearVelocity.y, rigid.linearVelocity.z);
        }
        if (inputDirection.y == 0 && Mathf.Abs(rigid.linearVelocity.z) > 0.001f)
        {
            float velocZ = rigid.linearVelocity.z;
            velocZ = Mathf.Lerp(velocZ, 0, 0.1f);
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, rigid.linearVelocity.y, velocZ);
        }

        if(OnGround() && inputDirection.magnitude == 0 && rigid.linearVelocity.y < 0)
        {
            coll.material.dynamicFriction = 0.2f;
        }
        else if(coll.material.dynamicFriction == 0.2f)
        {
            coll.material.dynamicFriction = 0f;
        }
    }

    void Flip()
    {
        isRight = !isRight;
        isTurning = true;
        bottom.DOLocalRotate(new Vector3(0, isRight ? -180 : 180, 0), 0.1f)
            .SetEase(Ease.Linear).SetRelative().OnComplete(() => isTurning = false);
    }

    void Jump()
    {
        if(OnGround())
        {
            rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
            rigid.AddForce(Vector3.up * jumpPow, ForceMode.Impulse);
            anim.SetTrigger("Jump");
        }
    }
    bool OnGround()
    {
        if (Physics.BoxCast(transform.position + gOffset, gSize / 2, Vector3.down, out gHit, Quaternion.identity, gDistance, gLayer))
        {
            return true;
        }
        else { return false; }
    }

    void Hold(bool isActivate)
    {
        if(isActivate && !isTurning && OnAttach())
        {
            isTurning = true;
            canJump = false;
            
            //joint.connectedBody = holdingObject;
        }
        else if(!isActivate)
        {
            isTurning = false;
            canJump = true;
        }
    }

    bool OnAttach()
    {
        if (Physics.BoxCast(transform.position + Vector3.up * 0.5f, hSize / 2, isRight ? Vector3.right : Vector3.left, 
            out hHit, Quaternion.identity, hDistance))
        {
            Debug.Log("Hold");
            return true;
        }
        else { return false; }
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint cp in collision.contacts)
        {
            Vector3 contact = cp.point;
            Vector3 normal = cp.normal;

            //Debug.Log(cp.normal);
            Debug.DrawRay(contact, normal, Color.yellow);
        }
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
        if (context.performed)
        {
            if(true)
            {
                //Hold(true);
                GameManager.Instance.book.Flip(isRight);
            }
            else
            {

            }
        }
        else if(context.canceled && true)
        {
            //Hold(false);
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
        Gizmos.DrawWireCube(transform.position + Vector3.up * 0.5f, new Vector3(0.5f, 1.0f, 0.05f));

        Gizmos.color = OnGround() ? Color.cyan : Color.red;
        Gizmos.DrawCube(transform.position + gOffset + Vector3.down * gDistance, gSize);

        Gizmos.color = OnAttach() ? Color.cyan : Color.red;
        Gizmos.DrawCube(transform.position + Vector3.up * 0.5f
                + new Vector3(isRight ? -hOffset.x : hOffset.x, hOffset.y, hOffset.z), hSize);
    }

}
