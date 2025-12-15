using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] TitleBookButton currentButton;
    [SerializeField] int currentIndex;
    [SerializeField] Button[] books;
    [SerializeField] Button[] select;
    [SerializeField] HGE03_MatFadeNew[] selectImages;
    [SerializeField] float fadeDuration;

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

    public void _SetCurrentButton(TitleBookButton button)
    {
        currentButton = button;
        currentIndex = button.i;
    }
    public void _ChangeToSelect(bool isSelect)
    {
        EventSystem.current.SetSelectedGameObject(select[0].gameObject);
    }

    public void _FadeSelectUI(bool fadeIn)
    {
        foreach (var i in selectImages)
        {
            i._StartFade(fadeIn);
        }
    }

    public void _Return()
    {
        EventSystem.current.SetSelectedGameObject(currentButton.gameObject);
    }
    public void _Confirm()
    {
        currentButton.OnConfirm();
        Invoke("ChangeScene", 0.5f);
    }
    void ChangeScene()
    {
        GameManager.Instance.ChangeScene(currentIndex);
    }
}
