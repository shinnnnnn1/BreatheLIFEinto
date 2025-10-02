using UnityEngine;
using DG.Tweening;

public class RHE01_FallingTree : MonoBehaviour
{
    [SerializeField] Transform tree;

    [SerializeField] Animator redhoodAnim;
    [SerializeField] Transform redhoodT;
    [SerializeField] Transform redhoodCollider;
    [SerializeField] Vector3 redhoodPos;
    [SerializeField] float redhoodHeight;
    [SerializeField] float redhoodtime;

    [SerializeField] Animator wolfAnim;
    [SerializeField] Transform wolfT;
    [SerializeField] Vector3 wolfPos;
    [SerializeField] float wolfHeight;
    [SerializeField] float wolfTime;
    [SerializeField] float wolfRunawayLength;
    [SerializeField] float wolfRunawayTime;

    public void WolfHit()
    {
        redhoodAnim.gameObject.SetActive(true);
        redhoodAnim.SetTrigger("Eaten");
        redhoodT.DOJump(redhoodPos, redhoodHeight, numJumps: 1, redhoodtime).SetEase(Ease.Linear);
    }

    public void WolfJump()
    {
        wolfT.DOJump(wolfPos, wolfHeight, numJumps: 2, wolfTime).SetEase(Ease.Linear);
    }

    public void WolfRunaway()
    {
        tree.gameObject.SetActive(false);
        redhoodCollider.gameObject.SetActive(true);

        wolfAnim.SetTrigger("Gundash");
        wolfT.DOMoveX(wolfRunawayLength, wolfRunawayTime).SetEase(Ease.Linear)
            .OnComplete(() => { wolfT.gameObject.SetActive(false); });
    }
}
