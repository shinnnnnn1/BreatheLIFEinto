using UnityEngine;
using DG.Tweening;

public class RHE15_FlyingAxe : MonoBehaviour
{
    [SerializeField] Transform axeT;
    [SerializeField] Vector3 pos;
    [SerializeField] float rot;
    [SerializeField] float height;
    [SerializeField] float time;

    public void FlyingAxe()
    {
        axeT.SetParent(null);
        axeT.DOJump(pos, height, 1, time).SetEase(Ease.Linear);
        axeT.DORotate(new Vector3(rot, 0, 0), time).SetEase(Ease.Linear).SetRelative();
    }
}
