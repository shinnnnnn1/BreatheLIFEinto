using System.Collections;
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

    [SerializeField] GameObject[] other;
    [SerializeField] EventSystem es;
    [SerializeField] bool isStarted = false;
    
    void Start()
    {
        Debug.Log("Title - Set Button");
        can = GameManager.Instance.canPlay;

        for(int i = 0; i < books.Length; i++)
        {
            books[i].enabled = can[i];
        }

        if (can[1])
        {
            SkipTitle();
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

    public void TitleStart()
    {
        if(!isStarted)
        {
            StartCoroutine(TitleStartCoroutine());
        }
    }

    IEnumerator TitleStartCoroutine()
    {
        isStarted = true;

        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(1);

        other[0].SetActive(false);
        other[1].SetActive(false);
        other[2].SetActive(true);

        //yield return null;

        FadeManager.Instance.FadeIn();

        Debug.Log("Title - Set Button");
        can = GameManager.Instance.canPlay;

        for (int i = 0; i < books.Length; i++)
        {
            books[i].enabled = can[i];
        }
        es.enabled = true;
    }

    public void SkipTitle()
    {
        int i = -1;
        foreach(bool c in can)
        {
            if (c) { i++; }
        }

        other[0].SetActive(false);  
        other[1].SetActive(false);
        other[2].SetActive(true);
        es.enabled = true;
        es.firstSelectedGameObject = books[i].gameObject;
        isStarted = true;
    }
}
