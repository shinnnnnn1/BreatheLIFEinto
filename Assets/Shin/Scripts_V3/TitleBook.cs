using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TitleBook : MonoBehaviour, ICursorInteractable
{
    [SerializeField] Transform model;
    [SerializeField] Vector3 enterPos, releasePos, movePos;
    [SerializeField] float enterDuration, releaseDuration, moveDuration;
    [SerializeField] Ease enterEase, releaseEase, moveEase;

    [Space(10f)] [SerializeField] bool isEntered;

    public void OnEnter()
    {
        if (isEntered) { return; }
        isEntered = true;

        transform.DOKill();
        model.DORotate(enterPos, enterDuration).SetEase(enterEase);
    }
    public void OnExit()
    {
        isEntered = false;

        transform.DOKill();
        model.DORotate(Vector3.zero, enterDuration).SetEase(enterEase);
    }
    public void OnPressed()
    {

    }
    public void OnReleased()
    {
        transform.DOKill();
        model.DORotate(releasePos, releaseDuration).SetEase(releaseEase);
        model.DOMove(movePos, moveDuration).SetEase(moveEase).SetRelative();
    }
    public void OnCanceled()
    {

    }
}
