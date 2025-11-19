using DG.Tweening;
using UnityEngine;

public class RHE03_DOJump : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] Vector3 pos;
    [SerializeField] float height;
    [SerializeField] int jumpNum;
    [SerializeField] float time;
    [SerializeField] Ease ease;

    public void _Jump()
    {
        trans.DOJump(pos, height, numJumps: jumpNum, time).SetEase(ease);
    }
}
