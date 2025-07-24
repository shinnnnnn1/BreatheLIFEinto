using UnityEngine;

public class Test2 : MonoBehaviour
{
    Rigidbody rigid;
    [SerializeField] bool a;

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
        if (!a) { return; }

        if (transform.position.x < 5.65f)
        {
            rigid.isKinematic = false;
            rigid.linearVelocity = new Vector3(2, rigid.linearVelocity.y, 0);
        }
        else if(transform.position.x > 5.65f)
        {
            rigid.isKinematic = true;
        }
        
    }
}
