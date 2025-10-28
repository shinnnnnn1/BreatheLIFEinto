using System.Linq;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 本の上のオブジェクトのクラス
/// </summary>
public class BaseObject : MonoBehaviour
{
    protected BookController controller;

    [Range(1, 10)] public int stage;

    [Space(30f)]
    public bool isRight;
    public float height;
    public Transform stand;

    [Space(10f)]
    public Transform closeBone;
    public int closeIndex;

    [Space(10f)]
    public bool isCurrent;
    public bool isStatic;
    public bool isActivate;

    [Space(10f)]
    public bool isLocked;

    [Space(10f)]
    public Collider[] coll;
    public Transform[] children;

    public virtual void Start()
    {
        //Controllernを参照
        controller = FindFirstObjectByType<BookController>();

        //色々なタイプに対応できるよう一番上の子を参照
        stand = transform.GetChild(0);

        //全てのColliderを参照
        coll = GetComponentsInChildren<Collider>();
        //全てのColliderを無効化
        EnableColliders(false);

        //オブジェクトの子を全て参照
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        //オブジェクトの子を全て無効化
        SetObjectVisible(false);

        //オブジェクト情報の初期設定
        SetBone();

        //高さ調整のための初期設定
        height = transform.position.y;
        stand.localPosition = (isRight ? Vector3.down : Vector3.up) * height * 2;
    }

    public void SetBone()
    {
        //Set IsRight
        isRight = transform.position.x > 0;

        //Set Close Bone and Index
        float dis = 100;
        Transform[] t = isRight ? controller.rightBones : controller.leftBones;
        for (int i = 0; i < t.Length; i++)
        {
            float close = Vector3.Distance(transform.position, t[i].position);
            if (close < dis)
            {
                dis = close;
                closeBone = t[i];
                closeIndex = i;
            }
        }
    }

    /// <summary>
    /// 一番近いBoneを親にする。初期設定専用
    /// </summary>
    public void SetParentStart()
    {
        transform.SetParent(closeBone);
    }

    /// <summary>
    /// 一番近いCurrentPageのBoneを親にする。
    /// </summary>
    public virtual void SetParent(Transform[] currentBones)
    {
        transform.SetParent(currentBones[closeIndex]);
    }

    /// <summary>
    /// 親子関係をリセットする
    /// </summary>
    /// <param name="objectParents">
    /// オブジェクトが戻る場所をステージ別に分けた配列
    /// </param>
    public virtual void ResetParent(Transform[] objectParents)
    {
        transform.SetParent(objectParents[stage]);
    }

    /// <summary>
    /// Colliderを全て有効、または無効にする
    /// </summary>
    public void EnableColliders(bool enable)
    {
        foreach (Collider c in coll)
        {
            c.enabled = enable;
        }
    }

    /// <summary>
    /// オブジェクトの子を全て表示、または非表示にする
    /// </summary>
    public void SetObjectVisible(bool visible)
    {
        foreach (Transform t in children)
        {
            //NPCの場合、会話のコライダーはコライダー全体の有効無効で制御している。
            if (t.name == "NPCCylinder") continue;

            t.gameObject.SetActive(visible);
        }
    }



    /// <summary>
    /// Stageに応じて動かすオブジェクトを決める
    /// </summary>
    /// <param name="currentStage"></param>
    /// <param name="currentBones"></param>
    public virtual void SetBookObject(int currentStage, Transform[] currentBones, Transform[] shapes, BookModel model)
    {
        if (stage == currentStage || stage == currentStage - 1)
        {
            //
            isCurrent = true;
            isActivate = stage == currentStage;
            isStatic = (isActivate && isRight) || (!isActivate && !isRight);

            //
            if(isActivate)
            {
                SetObjectVisible(true);
            }
            //
            else
            {
                EnableColliders(false);
            }

            //
            if (!isStatic)
            {
                SetParent(currentBones);
            }

            float heightValue = isActivate ? 0 : (isStatic ? -height : height);
            float heightTime = model.curveHeight[isActivate ? 0: 1].Evaluate(closeIndex);
            float heightDelay = model.curveHeight[isActivate ? 2 : 3].Evaluate(closeIndex);
            SetHeight(heightValue * 2, heightTime, heightDelay);
        }
        else
        {
            isCurrent = false;
        }
    }

    public virtual void SetHeight(float value, float time, float delay)
    {
        stand.DOLocalMoveY(value, time).SetDelay(delay)
            .SetEase(isRight ? Ease.OutQuint : Ease.OutQuint);
    }

    /// <summary>
    /// ページ移動が終わった後のオブジェクトの動作
    /// </summary>
    /// <param name="objectParents"></param>
    public virtual void AfterFlip(Transform[] objectParents)
    {
        ResetParent(objectParents);
        if (isActivate)
        {
            EnableColliders(true);
        }
        else
        {
            isCurrent = false;
            SetObjectVisible(false);
        }
    }
}
