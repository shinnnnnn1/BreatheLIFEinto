using DG.Tweening;
using System.Linq;
using UnityEngine;

public class NPCObject : BaseObject
{
    public MeshCollider npcCylinder;
    public bool[] isDirectional = new bool[] { true, true, true }; // -1, 0, 1

    public override void Start()
    {
        base.Start();
        stand.localEulerAngles = new Vector3(90, 0, 0);
        npcCylinder = GetComponentsInChildren<MeshCollider>().FirstOrDefault();
    }

    public override void SetBookObject(int currentStage, Transform[] currentBones, Transform[] shapes, BookModel model)
    {
        base.SetBookObject(currentStage, currentBones, shapes, model);
        if (isCurrent)
        {
            float rotValue = isActivate ? -90f : 90f;
            Vector3 rotVecter = new Vector3(rotValue, 0, 0);
            float rotTime = model.curvePlane[isActivate ? 0 : 1].Evaluate(closeIndex);
            float rotDelay = model.curvePlane[isActivate ? 2 : 3].Evaluate(closeIndex);
            SetPlaneRot(rotVecter, rotTime, rotDelay);
        }
    }

    void SetPlaneRot(Vector3 value, float time, float delay)
    {
        stand.DOLocalRotate(value, time).SetDelay(delay).SetRelative();
    }

    public override void AfterFlip(Transform[] objectParents)
    {
        base.AfterFlip(objectParents);
        if(isCurrent)
        {
            CheckDirectional(0);
        }
    }

    void Update()
    {
        if (isLocked)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public void CheckDirectional(int bookDir)
    {
        if(npcCylinder == null) { return; }

        if (isDirectional[bookDir + 1])
        {
            npcCylinder.enabled = false;
            npcCylinder.enabled = true;
        }
        else
        {
            npcCylinder.enabled = false;
        }
    }
}
