using UnityEngine;

public class RHE01_RedhoodStartMove : MonoBehaviour
{
    [SerializeField] bool isMovingRedHood;
    [SerializeField] Animator startRHAnim;
    [SerializeField] Rigidbody startRHRigid;

    public void StartRedHoodMoving(bool isMoving)
    {
        isMovingRedHood = isMoving;
        startRHRigid.isKinematic = !isMoving;
        startRHAnim.SetTrigger("Walk");
    }

    void FixedUpdate()
    {
        if(isMovingRedHood)
        {
            startRHRigid.linearVelocity = new Vector3(2, 0, 0);
        }
    }

    public void StartRedHoodDisable()
    {
        startRHRigid.gameObject.SetActive(false);
    }
}
