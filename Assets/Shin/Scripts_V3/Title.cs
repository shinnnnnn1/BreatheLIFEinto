using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] Button[] books;

    [SerializeField] bool[] can;
    
    void Start()
    {
        Debug.Log("Title - Set Button");
        can = GameManager.Instance.canPlay;

        for(int i = 0; i < books.Length; i++)
        {
            books[i].enabled = can[i];
        }
    }

}
