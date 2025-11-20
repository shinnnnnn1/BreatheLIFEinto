using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RHE06_DOLocalMove : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] Vector3 pos;
    [SerializeField] float time;
    [SerializeField] bool isRelative;
    [SerializeField] Ease ease;
    [SerializeField] UnityEvent onComplete;

    public void _LocalMove()
    {
        if(isRelative)
        {
            trans.DOLocalMove(pos, time).SetEase(ease).OnComplete(() => onComplete.Invoke());
        }
        else
        {
            trans.DOLocalMove(pos, time).SetEase(ease).SetRelative().OnComplete(() => onComplete.Invoke());
        }
    }
}
