using DG.Tweening;
using UnityEngine;

public class RHE04_RunawayWolf : MonoBehaviour
{
    [SerializeField] Transform wolfT;
    [SerializeField] float wolfRunawayLength;
    [SerializeField] float wolfRunawayTime;

    public void WolfRunaway()
    {
        wolfT.DOMoveX(wolfRunawayLength, wolfRunawayTime).SetEase(Ease.Linear).SetRelative()
            .OnComplete(() => { wolfT.gameObject.SetActive(false); });
    }
}
