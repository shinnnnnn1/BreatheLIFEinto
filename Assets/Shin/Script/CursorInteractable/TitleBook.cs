using UnityEngine;
using DG.Tweening;

public class TitleBook : MonoBehaviour, ICursor
{
    [SerializeField] [Range(0, 9)] int book;

    Title title;
    bool isEnter;

    void Start()
    {
        title = FindAnyObjectByType<Title>();
    }

    public void OnEnter()
    {
        if (isEnter) { return; }
        transform.DOKill();
        float time = (transform.eulerAngles.x - 340) / 20;
        transform.DORotate(new Vector3(-20, 0, 0), time / 3).SetEase(Ease.Linear);
        isEnter = true;
    }

    public void OnExit()
    {
        if (!isEnter) { return; }
        transform.DOKill();
        float time = (360 - transform.eulerAngles.x) / 20;
        transform.DORotate(new Vector3(-0.01f, 0, 0), time / 3).SetEase(Ease.Linear);
        isEnter = false;
    }

    public void OnActivate()
    {
        title.NextScene(book);
        
    }

    public void OnDeactivate()
    {

    }

    void Update()
    {

    }
}
