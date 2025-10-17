using UnityEngine;
using DG.Tweening;

public class RHE20_HunterInHouse : MonoBehaviour
{
    [SerializeField] Transform t;
    [SerializeField] Vector3 pos;
    [SerializeField] float height;
    [SerializeField] float time;

    public void StartMove()
    {
        t.DOJump(pos, height, 1, time).SetEase(Ease.Linear);
    }
}
