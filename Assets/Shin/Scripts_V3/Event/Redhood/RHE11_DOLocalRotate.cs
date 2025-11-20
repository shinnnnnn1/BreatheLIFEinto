using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RHE11_DOLocalRotate : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] Vector3 rot;
    [SerializeField] float time;
    [SerializeField] bool isRelative;
    [SerializeField] Ease ease;
    [SerializeField] UnityEvent onComplete;

    public void _LocalRotate()
    {
        if (isRelative)
        {
            trans.DOLocalRotate(rot, time).SetEase(ease).OnComplete(() => onComplete.Invoke());
        }
        else
        {
            trans.DOLocalRotate(rot, time).SetEase(ease).SetRelative().OnComplete(()=> onComplete.Invoke());
        }
    }
}
