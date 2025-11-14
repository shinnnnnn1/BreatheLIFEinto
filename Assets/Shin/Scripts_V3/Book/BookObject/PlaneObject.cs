using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// FlipでStandの回転をするタイプ
/// </summary>
public class PlaneObject : BaseObject_V3
{
    [Space(10)]
    //standはFlip用、planeはTurn用
    public Transform plane;
    public Animator anim;
    public MeshCollider npcCylinder;
    public bool[] isDirectional = new bool[] { true, true, true }; // -1, 0, 1

    [Space(10)]
    public bool isFacingRight;

    public override void Start()
    {
        base.Start();

        //
        stand.DOLocalRotate(new Vector3(90, 0, 0), 0);

        //
        plane = stand.GetChild(0);
        anim = plane.GetComponent<Animator>();

        //처음에 안보이게할거는
        npcCylinder = GetComponentsInChildren<MeshCollider>().FirstOrDefault();

        isFacingRight = plane.localEulerAngles.y < 90;
    }

    /// <summary>
    /// オブジェクトのタイプごとにモーションを実行。
    /// </summary>
    /// <seealso cref="BookController_V3.Flip()"/>
    public override void FlipMotion(BookModel_V3 model, bool isAct)
    {
        base.FlipMotion(model, isAct);

        if(isCurrent && isAct == isActivate)
        {
            //PlaneObjectの場合、Standを回転させる
            float value = isAct ? -90 : 90;
            Vector3 rotValue = new Vector3(value, 0, 0);
            float time = model.curvePlane[isAct ? 0 : 1].Evaluate(closeIndex);
            float delay = model.curvePlane[isAct ? 2 : 3].Evaluate(closeIndex);
            Ease ease = model.easePlane[isAct ? 0 : 1];

            stand.DOLocalRotate(rotValue, time).SetDelay(delay).SetEase(ease).SetRelative();
        }
    }



    public void Turn()
    {
        Turn(!isFacingRight);
    }
    public void Turn(bool turnToRight)
    {
        if ((turnToRight && isFacingRight) || (!turnToRight && !isFacingRight)) { return; }

        float turnValue = turnToRight ? -180 : 180;
        plane.DORotate(new Vector3(0, turnValue, 0), 0.2f).SetEase(Ease.Linear).SetRelative();
        isFacingRight = turnToRight;
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



    public void SetAnimTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }
    public void SetAnimTriggerWithTurn(string trigger)
    {
        StartCoroutine(AnimTime(trigger));
    }

    IEnumerator AnimTime(string trigger)
    {
        yield return new WaitForSeconds(0.1f);
        SetAnimTrigger(trigger);
    }
}
