using UnityEngine;

public class RHE10_RedhoodCross : MonoBehaviour
{
    [SerializeField] bool isMoving;
    [SerializeField] float rhSpeed;
    [SerializeField] Rigidbody rhRigid;

    public void StartRedhoodMoving_CrossTheBridge()
    {
        isMoving = true;
        rhRigid.isKinematic = false;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            rhRigid.linearVelocity = new Vector3(rhSpeed, 0, 0);
        }
    }
}
