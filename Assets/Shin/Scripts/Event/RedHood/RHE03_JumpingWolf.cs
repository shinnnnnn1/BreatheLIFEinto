using DG.Tweening;
using UnityEngine;

public class RHE03_JumpingWolf : MonoBehaviour
{
    [SerializeField] Transform wolfT;
    [SerializeField] Vector3 wolfPos;
    [SerializeField] float wolfHeight;
    [SerializeField] float wolfTime;

    public void WolfJump()
    {
        wolfT.DOJump(wolfPos, wolfHeight, numJumps: 2, wolfTime).SetEase(Ease.Linear);
    }
}
