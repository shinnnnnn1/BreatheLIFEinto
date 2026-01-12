using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class HGE19_DOLocalScale : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] Vector3 scale;
    [SerializeField] float time;
    [SerializeField] bool isRelative;
    [SerializeField] Ease ease;
    [SerializeField] UnityEvent onComplete;

    public void _LocalScale()
    {
        if (isRelative)
        {
            trans.DOScale(scale, time).SetEase(ease).OnComplete(() => onComplete.Invoke());
        }
        else
        {
            trans.DOScale(scale, time).SetEase(ease).SetRelative().OnComplete(() => onComplete.Invoke());
        }
    }
}
