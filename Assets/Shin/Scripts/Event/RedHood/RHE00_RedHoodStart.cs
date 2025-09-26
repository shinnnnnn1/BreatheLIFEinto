using UnityEngine;

public class RHE00_RedHoodStart : MonoBehaviour
{
    [SerializeField] bool isMovingRedHood;
    [SerializeField] Rigidbody startRedHood;

    public void StartRedHoodMoving(bool isMoving)
    {
        isMovingRedHood = isMoving;
        startRedHood.isKinematic = !isMoving;
    }

    void FixedUpdate()
    {
        if(isMovingRedHood)
        {
            startRedHood.linearVelocity = new Vector3(2, 0, 0);
        }
    }

    public void StartRedHoodDisable()
    {
        startRedHood.gameObject.SetActive(false);
    }
}
