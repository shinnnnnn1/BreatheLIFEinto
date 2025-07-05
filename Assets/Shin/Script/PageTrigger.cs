using UnityEngine;

public class PageTrigger : MonoBehaviour
{
    public bool canFlip;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //GameManager.Instance.book.Flip();
        }
    }
}
