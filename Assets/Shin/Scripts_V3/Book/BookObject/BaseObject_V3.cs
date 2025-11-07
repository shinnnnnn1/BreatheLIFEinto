using DG.Tweening;
using UnityEngine;

public class BaseObject_V3 : MonoBehaviour, IBookObject
{
    protected IBookController bookController;

    [Range(1, 10)] [SerializeField] int stage;

    [Space(10f)]
    [SerializeField] bool isRight;
    [SerializeField] float height;
    [SerializeField] Transform stand;

    Transform[] pageL, pageR, pageLC, pageRC;

    [Space(10f)]
    [SerializeField] Transform closeBone;
    [SerializeField] int closeIndex;

    [Space(10f)]
    [SerializeField] bool isCurrent;
    [SerializeField] bool isStatic;
    [SerializeField] bool isActivate;

    [SerializeField] Collider[] coll;
    [SerializeField] Transform[] children;

    [Space(10f)]
    [SerializeField] float heightDelay;

    public virtual void Start()
    {
        //Controllernを参照
        bookController = GameObject.FindGameObjectWithTag("BookController").GetComponent<IBookController>();

        //色々なタイプに対応できるように一番上の子を参照
        stand = transform.GetChild(0);

        //全てのColliderを参照
        coll = GetComponentsInChildren<Collider>();
        //全てのColliderを無効化
        EnableColliders(false);

        //全ての子を参照
        children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        //全ての子を無効化
        SetObjectVisible(false);

        //オブジェクト情報の初期設定
        SetBone();

        //高さ調整のための初期設定
        height = transform.position.y;
        stand.localPosition = (isRight ? Vector3.down : Vector3.up) * height * 2;
    }

    #region SETUPーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// 本からボーンをもらう。初期設定専用
    /// </summary>
    /// <seealso cref="BookController_V3.Start()"/>
    public void GetBones(Transform[] pL, Transform[] pR, Transform[] pLC, Transform[] pRC)
    {
        pageL = pL;
        pageR = pR;
        pageLC = pLC;
        pageRC = pRC;
    }
    /// <summary>
    /// 一番近いBoneを親にする。初期設定専用
    /// </summary>
    /// <seealso cref="BookController_V3.Start()"/>
    public void SetStartParent()
    {
        if (!isRight) { transform.SetParent(closeBone); }
    }
    #endregion

    #region SET
    /// <summary>
    /// 現在位置と一番近いボーンを取得。初期設定や位置が変わるオブジェクトのFlipで実行
    /// </summary>
    /// <seealso cref="BookController_V3.Start()"/>
    public void SetBone()
    {
        //IsRightを設定
        isRight = transform.position.x > 0;

        //一番近いボーンを取得
        float dis = 100;
        Transform[] t = isRight ? pageR : pageL;
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
    /// 一番近いCurrentPageのBoneを親にする。
    /// </summary>
    public virtual void SetParent(Transform[] currentBones)
    {
        transform.SetParent(currentBones[closeIndex]);
    }

    /// <summary>
    /// 親子関係をリセットする
    /// </summary>
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
    #endregion

    #region FLIP
    //★★★★★★★★★★★★★★★★★★처음에 세팅해놓는건 낡은거 새로운거 둘다 함
    //★★★★★★★★★★★★★★★★★★근데 그 후에 높이조절이나 애니메이션, 셰잎, 플레인은 선, 후 나눠야됨.
    /// <summary>
    /// Stageに合わせて動かすオブジェクトを決める
    /// </summary>
    /// <seealso cref="BookController_V3.Flip()"/>
    public virtual void SetBookObject(int currentStage, BookModel_V3 model)
    {
        //古い、新しいStageのオブジェクトなら
        if (stage == currentStage || stage == currentStage - 1)
        {
            //今回Flipしたかを設定
            isCurrent = true;
            //新しいオブジェクトかを設定
            isActivate = stage == currentStage;
            //ページに追従する、しないを設定
            isStatic = (isActivate && isRight) || (!isActivate && !isRight);

            //開くオブジェクトを表示にする
            if (isActivate)
            {
                SetObjectVisible(true);
            }
            //古いオブジェクトならコライダーを無効化する
            else
            {
                EnableColliders(false);
            }

            //Dynamicの場合、Flipするページの子になる。古いのはRC、新しいのはLC
            if (!isStatic)
            {
                Transform[] newBones = isActivate ? pageLC : pageRC;
                SetParent(newBones);
            }

            //Heightの調整
            float hValue = isActivate ? 0 : (isStatic ? -height : height) * 2;
            float hTime = model.curveHeight[isActivate ? 0 : 1].Evaluate(closeIndex);
            float hDelay = model.curveHeight[isActivate ? 2 : 3].Evaluate(closeIndex) + heightDelay;
            SetHeight(hValue, hTime, hDelay);
        }
    }

    public virtual void FlipDeactivateObject()
    {

    }
    public virtual void FlipActivateObject()
    {

    }

    /// <summary>
    /// Heightの調整
    /// </summary>
    /// <seealso cref="SetBookObject(int, BookModel_V3)"/>
    public virtual void SetHeight(float value, float time, float delay)
    {
        stand.DOLocalMoveY(value, time).SetDelay(delay).SetEase(isActivate ? Ease.OutQuint : Ease.InQuint);
    }

    /// <summary>
    /// ページ移動が終わった後のオブジェクトの動作
    /// </summary>
    /// <seealso cref="BookController_V3.Flip()"/>
    public virtual void AfterFlip(Transform[] objectParents)
    {
        //今回Flipしたオブジェクトだけ実行
        if(isCurrent)
        {
            //親子関係をリセットする
            ResetParent(objectParents);

            //新しいオブジェクトならコライダーを有効化する
            if (isActivate)
            {
                EnableColliders(true);
            }
            //古いオブジェクトならisCurrentをfalseにし、非表示する
            else
            {
                isCurrent = false;
                SetObjectVisible(false);
            }
        }
    }
    #endregion
}
