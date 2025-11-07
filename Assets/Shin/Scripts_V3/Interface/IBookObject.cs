using UnityEngine;

public interface IBookObject
{
    public void GetBones(Transform[] pL, Transform[] pR, Transform[] pLC, Transform[] pRC);
    public void SetStartParent();
    public void ResetParent(Transform[] objectParents);
    public void SetBookObject(int currentStage, BookModel_V3 model);
    public void AfterFlip(Transform[] objectParents);
}
