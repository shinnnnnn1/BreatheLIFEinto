using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

/// <summary>
/// FlipでStandの回転をするタイプ。主にNPC
/// </summary>
public class PlaneObject : BaseObject_V3
{
    [Space(10)]
    //standはFlip用、planeはTurn用
    public Transform plane;
    public Animator anim;
    public NavMeshAgent navMeshAgent;

    public MeshCollider npcCylinder;
    [Space(10f)]
    public Image eventImage;
    public bool[] isDirectional = new bool[] { true, true, true, true, true }; // -1, 0, 1

    [Space(10)]
    public bool isFacingRight;

    [SerializeField] bool isLocked;

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

        navMeshAgent = GetComponent<NavMeshAgent>();

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
            //NasMeshAgentを無効化する
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }

            //PlaneObjectの場合、Standを回転させる
            float value = isAct ? -90 : 90;
            Vector3 rotValue = new Vector3(value, 0, 0);
            float time = model.curvePlane[isAct ? 0 : 1].Evaluate(closeIndex);
            float delay = model.curvePlane[isAct ? 2 : 3].Evaluate(closeIndex);
            Ease ease = model.easePlane[isAct ? 0 : 1];

            stand.DOLocalRotate(rotValue, time).SetDelay(delay).SetEase(ease).SetRelative();
        }
    }

    public override void AfterFlip(Transform[] objectParents)
    {
        base.AfterFlip(objectParents);
        if (isCurrent && isActivate)
        {
            //NasMeshAgentを有効化する
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = true;
            }

            LockObject(false, 0);
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
        plane.DOPause();
        plane.DORotate(new Vector3(0, turnValue, 0), 0.2f).SetEase(Ease.Linear).SetRelative();
        isFacingRight = turnToRight;
    }
    public void TurnToPlayer(Transform player)
    {
        Transform playerpos = player;
        bool isOverPlayer = transform.position.x > playerpos.position.x;
        if ((!isOverPlayer && !isFacingRight) || (isOverPlayer && isFacingRight))
        {
            Turn();
        }
    }
    public void TurnAlwwaysToPlayer(Transform player)
    {
        Transform playerpos = player;
        bool isOverPlayer = transform.position.x > playerpos.position.x;
        if ((!isOverPlayer && !isFacingRight) || (isOverPlayer && isFacingRight))
        {
            Turn();
        }
        else
        {
            StartCoroutine(Tatp());
        }
    }
    IEnumerator Tatp()
    {

        float turnValue = isFacingRight ? -90 : 90;
        plane.DORotate(new Vector3(0, turnValue, 0), 0.1f).SetEase(Ease.Linear).SetRelative();

        yield return new WaitForSeconds(0.1f);

        Vector3 newRot = new Vector3(0, 90, 0);
        plane.localEulerAngles = newRot;

        plane.DORotate(new Vector3(0, turnValue, 0), 0.1f).SetEase(Ease.Linear).SetRelative();
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



    public override void LockObject(bool onLock, int bookDir)
    {
        if(isCurrent)
        {
            isLocked = onLock;
            if (navMeshAgent != null) { navMeshAgent.enabled = !onLock; }

            if(onLock && npcCylinder != null)
            {
                npcCylinder.gameObject.SetActive(false);
                eventImage?.gameObject.SetActive(false);
            }
            else if (!onLock && npcCylinder != null)
            {
                npcCylinder.gameObject.SetActive(isDirectional[bookDir + 2]);
            }
        }
    }
    private void Update()
    {
        if (isLocked)
        {
            transform.rotation = Quaternion.identity;
        }
    }

}
