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

    [SerializeField] ManualOutlineFlash[] flashs;
    [SerializeField] int iii;
    
    void Start()
    {
        Debug.Log("Title - Set Button");
        can = GameManager.Instance.canPlay;

        for(int i = 0; i < books.Length; i++)
        {
            books[i].enabled = can[i];
        }

        if (can[3])
        {
            can[0] = true;
            can[1] = false;
            can[2] = false;
            can[3] = false;
        }
        else if (can[1])
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
        es.enabled = false;
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
        iii = i;

        other[0].SetActive(false);  
        other[1].SetActive(false);
        other[2].SetActive(true);
        es.enabled = true;
        es.firstSelectedGameObject = books[i].gameObject;
        isStarted = true;

        if(i == 1) { books[2].enabled = true; }
        StartCoroutine(StartFlash());
    }

    IEnumerator StartFlash()
    {
        books[0].interactable = false;
        books[1].interactable = false;
        books[2].interactable = false;

        Navigation nav = books[0].navigation;
        nav.selectOnRight = null;
        books[0].navigation = nav;

        nav = books[1].navigation;
        nav.selectOnLeft = null;
        nav.selectOnRight = null;
        books[1].navigation = nav;

        nav = books[2].navigation;
        nav.selectOnLeft = null;
        books[2].navigation = nav;

        foreach(var f in flashs)
        {
            f._StartFlash();
        }

        yield return new WaitForSeconds(2f);

        books[0].interactable = true;
        books[1].interactable = true;
        books[2].interactable = true;

        nav = books[0].navigation;
        nav.selectOnRight = books[1];
        books[0].navigation = nav;

        nav = books[1].navigation;
        nav.selectOnLeft = books[0];
        nav.selectOnRight = books[2];
        books[1].navigation = nav;

        nav = books[2].navigation;
        nav.selectOnLeft = books[1];
        books[2].navigation = nav;

        for(int i = 0; i < flashs.Length; i++)
        {
            if(i != iii)
            {
                flashs[i]._StopFlash(true);
            }
        }
    }

    public void FlashButton()
    {
        //books[i]
    }
}
