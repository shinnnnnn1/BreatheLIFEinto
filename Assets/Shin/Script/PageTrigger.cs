using UnityEngine;

public class PageTrigger : MonoBehaviour
{
    Book book;
    [SerializeField] bool isRightPage;

    void Start()
    {
        book = FindAnyObjectByType<Book>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.Instance.book.Flip(isRightPage);
        }
    }
}
