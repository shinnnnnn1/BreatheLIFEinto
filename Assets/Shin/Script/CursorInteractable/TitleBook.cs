using UnityEngine;
using DG.Tweening;

public class TitleBook : MonoBehaviour, ICursorInteractable
{
    [SerializeField] [Range(0, 9)] int book;

    bool isEnter;

    public void OnEnter()
    {
        if (isEnter) { return; }

        float time = (transform.eulerAngles.x == 0 ? 20 : transform.eulerAngles.x - 340) / 20;
        transform.DORotate(new Vector3(-20, 0, 0), time).SetEase(Ease.Linear);

        isEnter = true;
    }
    public void OnExit()
    {
        if (!isEnter) { return; }

        float time = (360 - transform.eulerAngles.x) / 20;
        transform.DORotate(new Vector3(0, 0, 0), time).SetEase(Ease.Linear);

        isEnter = false;
    }

    public void OnActivate()
    {

    }
}
