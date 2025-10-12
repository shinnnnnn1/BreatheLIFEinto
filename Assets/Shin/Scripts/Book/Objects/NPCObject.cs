using DG.Tweening;
using System.Linq;
using UnityEngine;

public class NPCObject : BaseObject
{
    [Space(10)]
    public Transform plane;
    public Animator anim;
    public MeshCollider npcCylinder;
    public bool[] isDirectional = new bool[] { true, true, true }; // -1, 0, 1

    [Space(10)]
    public bool isFacingRight;

    public override void Start()
    {
        base.Start();
        stand.localEulerAngles = new Vector3(90, 0, 0);

        plane = stand.GetChild(0);
        anim = plane.GetComponent<Animator>();
        npcCylinder = GetComponentsInChildren<MeshCollider>().FirstOrDefault();

        isFacingRight = plane.localEulerAngles.y < 90;
    }

    public override void SetBookObject(int currentStage, Transform[] currentBones, Transform[] shapes, BookModel model)
    {
        //아마 이게 NPC가 옆으로 이동해도 가까운 본을 다시 찾아서 페이지 넘길때 알맞은 위치에서 되게하는거.
        if (isCurrent)
        {
            SetBone();
        }


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

    public void Turn()
    {
        Turn(!isFacingRight);
    }

    public void TurnToPlayer()
    {
        Transform playerpos = EventManager.Instance.playerController.transform;
        bool isOverPlayer = transform.position.x > playerpos.position.x;
        if ((!isOverPlayer && !isFacingRight) || (isOverPlayer && isFacingRight))
        {
            Turn();
        }
    }

    public void Turn(bool turnToRight)
    {
        if((turnToRight && isFacingRight) || (!turnToRight && !isFacingRight)) { return; }

        float turnValue = turnToRight ? -180 : 180;
        plane.DORotate(new Vector3(0, turnValue, 0), 0.2f).SetEase(Ease.Linear).SetRelative();
        isFacingRight = turnToRight;
    }

    public void SetAnimTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void SetDisableDialogue()
    {
        npcCylinder?.gameObject.SetActive(false);
    }

}
