using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class HGE40_DOPath : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] Vector3[] paths;
    [SerializeField] float duration;
    [SerializeField] Ease ease;
    [SerializeField] UnityEvent onComplete;

    public void _StartPath()
    {
        trans.DOPath(paths, duration, PathType.CatmullRom).SetEase(ease)
            .OnComplete(() => onComplete.Invoke());
    }
}
