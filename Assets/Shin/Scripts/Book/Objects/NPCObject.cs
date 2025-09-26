using UnityEngine;
using DG.Tweening;

public class NPCObject : BaseObject
{
    [SerializeField] bool canEvent;
    IEventInvoker eventInvoker;

    public override void Start()
    {
        base.Start();
        stand.localEulerAngles = new Vector3(90, 0, 0);
        eventInvoker = GetComponent<IEventInvoker>();
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



    void OnTriggerEnter(Collider other)
    {
        if(!canEvent && eventInvoker != null)
        {
            canEvent = true;
            eventInvoker.CanStartEvent(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (canEvent && eventInvoker != null)
        {
            canEvent = false;
            eventInvoker.CanStartEvent(false);
        }
    }
}
