using UnityEngine;
using DG.Tweening;

public class RHE02_FallingTree : MonoBehaviour
{
    [SerializeField] Transform redhoodT;
    [SerializeField] Vector3 redhoodPos;
    [SerializeField] float redhoodHeight;
    [SerializeField] float redhoodtime;

    public void WolfHit()
    {
        redhoodT.DOJump(redhoodPos, redhoodHeight, numJumps: 1, redhoodtime).SetEase(Ease.Linear);
    }
}
