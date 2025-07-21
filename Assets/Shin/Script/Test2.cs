using UnityEngine;

public class Test2 : MonoBehaviour
{
    Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
    }

    private void Update()
    {
        if (transform.position.x < 5.65f)
        {
            rigid.linearVelocity = new Vector3(2, 0, 0);
        }
        
    }
}
