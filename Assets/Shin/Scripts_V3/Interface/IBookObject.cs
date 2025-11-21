using DG.Tweening;
using UnityEngine;

public interface IBookObject
{
    public void GetBones(Transform[] pL, Transform[] pR, Transform[] pLC, Transform[] pRC, Transform[] sA, Transform[] sD);
    public void SetStartParent();
    public void ResetParent(Transform[] objectParents);
    public void SetBookObject(int currentStage);
    public void FlipMotion(BookModel_V3 model, bool isAct);
    public void FlipHeight(BookModel_V3 model, bool isAct);











    public void AfterFlip(Transform[] objectParents);
    public void LockObject(bool onLock, int bookDir);
}
