using DG.Tweening;
using UnityEngine;

public class RHE06_DOLocalMove : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] Vector3 pos;
    [SerializeField] float time;
    [SerializeField] Ease ease;

    public void _LocalMove()
    {
        trans.DOLocalMove(pos, time).SetEase(ease).SetRelative();
    }
}
