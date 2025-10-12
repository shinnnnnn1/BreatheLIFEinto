using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// 本を操作するクラス
/// </summary>
public class BookController : MonoBehaviour
{
    [SerializeField] BookModel model;
    [SerializeField] Transform leftBone, rightBone, currentBone, objectParent, shape;

    //shape[0 ~ 9] = Activate, shape[10 ~ 18] = Deactivate
    //[HideInInspector] 
    public Transform[] leftBones, rightBones, currentBones, objectParents, shapes;

    [Space(10f)]
    [SerializeField] int currentPage = 0;
    [SerializeField] float flipTime;
    public bool isFlipping = false;
    float cTime;

    [Space(10f)]
    [SerializeField] BaseObject[] bookObjects;

    BookView view;
    IBeforeAfterFlip beforeAfterFlip;
    PlayerController player;
    FlipTriggerController flipController;

    public int bookDir = 0;

    public virtual void Awake()
    {
        //開発用。スタート地点を設定できる。すごい！
        currentPage = model.setStartPage - 1;

        //BookのView, BookAfterFlipを参照
        view = GetComponent<BookView>();
        beforeAfterFlip = GetComponent<IBeforeAfterFlip>();
        player = FindFirstObjectByType<PlayerController>();
        flipController = FindFirstObjectByType<FlipTriggerController>();

        //PageのBoneを参照
        leftBones = leftBone.GetComponentsInChildren<Transform>();
        rightBones = rightBone.GetComponentsInChildren<Transform>();
        currentBones = currentBone.GetComponentsInChildren<Transform>();

        //本のオブジェクトが置かれる場所を参照
        objectParents = new Transform[objectParent.childCount];
        for (int i = 0; i < objectParent.childCount; i++)
        {
            objectParents[i] = objectParent.GetChild(i);
        }

        //Shapeオブジェクトを参照
        shapes = shape.GetComponentsInChildren<Transform>().Where(w => w != shape).ToArray();

        //本の上のオブジェクトを全て参照
        bookObjects = objectParent.GetComponentsInChildren<BaseObject>();

        //view.SetAllPageVisibility(false);
    }

    /// <summary>
    /// シーン上のオブジェクトを使用可能にするための工程
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        //最初は0ページから始まるため、左にあるオブジェクトを固定し、右にめめくる
        foreach (var obj in bookObjects)
        {
            if(!obj.isRight)
            {
                obj.SetParentStart();
            }
        }

        yield return new WaitForSeconds(0.2f);

        //アニメーションの速度を速くして
        view.SetAnimationSpeed(0, 20f);
        //初期設定専用のアニメーションを再生
        view.PlayPageAnimation(0, "Start");

        yield return new WaitForSeconds(0.3f);

        //本に親子付けしたオブジェクトをリセット
        foreach (var obj in bookObjects)
        {
            if (!obj.isRight)
            {
                obj.ResetParent(objectParents);
            }
        }

        yield return new WaitForSeconds(0.1f);

        //ページのアニメーションも元通りに戻す
        view.PlayPageAnimation(0, "Reset");

        yield return new WaitForSeconds(0.3f);

        //アニメーションの速度も戻す
        view.SetAnimationSpeed(0, 1f);

        Debug.Log("Start");
        player.SetCanGameStart();
    }

    public void BookOpen()
    {
        StartCoroutine(OpenCoroutine());
    }
    IEnumerator OpenCoroutine()
    {
        yield return null;
    }

    void Update()
    {
        if (isFlipping)
        {
            flipTime = Time.time - cTime;
        }
    }

    /// <summary>
    /// ページ移動
    /// </summary>
    public void Flip()
    {
        if (isFlipping) { return; }

        isFlipping = true;
        currentPage++;
        Debug.Log($"Currently on Page [ {currentPage} ]");

        cTime = Time.time;

        StartCoroutine(FlipCoroutine());
    }

    IEnumerator FlipCoroutine()
    {
        beforeAfterFlip.OnBeforeFlip(currentPage, out int beforeWaitTime);
        yield return new WaitForSeconds(beforeWaitTime);

        view.SetPageVisibility(true, false);
        StartShape();
        foreach (var obj in bookObjects)
        {
            obj.SetBookObject(currentPage, currentBones, shapes, model);
        }

        yield return null;

        view.PlayPageAnimation(2, "Flip");
        view.PlayPageAnimation(3, "Reverse");

        yield return new WaitForSeconds(1.25f);

        view.SetPageVisibility(false, true);
        view.SetPageMaterial(currentPage);

        yield return new WaitForSeconds(1.75f);

        foreach (var obj in bookObjects)
        {
            obj.AfterFlip(objectParents);
        }
        view.PlayPageAnimation(2, "Reset");
        isFlipping = false;

        //Reset Morph
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].DOPause();
            shapes[i].position = new Vector3(shapes[i].position.x, i < 9 ? 0 : 1, shapes[i].position.z);
        }

        beforeAfterFlip.OnAfterFlip(currentPage);
        view.SetPageVisibility(false, false);

        flipController.SetCanProceed(false);
    }

    void StartShape()
    {
        for (int i = 0; i < 9; i++)
        {
            float time = model.curveShape[0].Evaluate(i);
            float delay = model.curveShape[2].Evaluate(i);
            shapes[i].DOLocalMoveY(1, time).SetDelay(delay).SetEase(Ease.InOutQuad);
        }
        for (int i = 9; i < 18; i++)
        {
            float time = model.curveShape[1].Evaluate(i - 10);
            float delay = model.curveShape[3].Evaluate(i - 10);
            shapes[i].DOLocalMoveY(0, time).SetDelay(delay).SetEase(Ease.InOutQuad);
        }
    }

    public void TurnBook(bool isRightTurn, out bool canTurn)
    {
        //bool canRotate = (isRightTurn && bookDir != 1) && ();

        if((isRightTurn && bookDir == 1) || (!isRightTurn && bookDir == -1)) { canTurn = false; return; }
        canTurn = true;

        bookDir = isRightTurn ? bookDir + 1 : bookDir - 1;

        float rot = isRightTurn ? model.rotValue : -model.rotValue;
        view.TurnBookAnimation(isRightTurn, rot, model.rotTime);

        LockObjects(true);

        Invoke("AfterTurnBook", model.rotTime);
    }

    void AfterTurnBook()
    {
        LockObjects(false);
        player.LockPlayer(false);
        flipController.CheckBookIsHorizontal(bookDir);
    }

    void LockObjects(bool onLock)
    {
        foreach(BaseObject b in bookObjects)
        {
            if(b.isCurrent)
            {
                b.isLocked = onLock;
                if(!onLock)
                {
                    NPCObject n = b.GetComponent<NPCObject>();
                    n?.CheckDirectional(bookDir);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (leftBones != null)
        {
            foreach (Transform t in leftBones)
            {
                Gizmos.DrawSphere(t.position, 0.05f);
            }
            foreach (Transform t in rightBones)
            {
                Gizmos.DrawSphere(t.position, 0.05f);
            }
            foreach (Transform t in currentBones)
            {
                Gizmos.DrawSphere(t.position, 0.05f);
            }
        }
    }
}
