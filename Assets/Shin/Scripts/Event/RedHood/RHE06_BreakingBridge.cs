using UnityEngine;
using DG.Tweening;

public class RHE06_BreakingBridge : MonoBehaviour
{
    [SerializeField] Transform wolfS;
    [SerializeField] Transform wolfP;

    [SerializeField] Vector3 nextPos;
    [SerializeField] Vector3 nextRot;
    [SerializeField] float loopTime;
    [SerializeField] AnimationCurve curve;

    [SerializeField] RectTransform wolfD;
    [SerializeField] Vector3 newDPos;

    public void WolfBreakingBridge()
    {
        wolfS.DOLocalMove(nextPos, loopTime).SetLoops(-1, LoopType.Yoyo).SetEase(curve);
        wolfP.DORotate(nextRot, loopTime).SetLoops(-1, LoopType.Yoyo).SetEase(curve);
    }

    public void StopBreaking()
    {
        wolfS.DOPause();
        wolfP.DOPause();
        wolfS.localPosition = Vector3.zero;
        wolfP.localEulerAngles = new Vector3(0, 180, 0);
        wolfD.localPosition = newDPos;
    }
}
