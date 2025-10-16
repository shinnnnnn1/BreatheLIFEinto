using UnityEngine;

public class RHE01_RedhoodStartMove : MonoBehaviour
{
    [SerializeField] bool isMovingRedHood;
    [SerializeField] Rigidbody startRHRigid;

    public void StartRedHoodMoving(bool isMoving)
    {
        isMovingRedHood = isMoving;
        startRHRigid.isKinematic = !isMoving;
    }

    void FixedUpdate()
    {
        if(isMovingRedHood)
        {
            startRHRigid.linearVelocity = new Vector3(3, 0, 0);
        }
    }

    public void StartRedHoodDisable()
    {
        startRHRigid.gameObject.SetActive(false);
    }
}
