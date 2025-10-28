using DG.Tweening;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.AI;

/// <summary>
/// 本を操作するクラス
/// </summary>
public class BookController : MonoBehaviour
{
    [SerializeField] BookModel model;
    [SerializeField] Transform leftBone, rightBone, currentBone, objectParent, shape;

    //shape[0 ~ 9] = Activate, shape[10 ~ 18] = Deactivate
    [HideInInspector] 
    public Transform[] leftBones, rightBones, currentBones, objectParents, shapes;

    [Space(10f)]
    public int currentPage = 0;
     bool isFlipping = false;

    [SerializeField] float flipTime;
    float cTime;

    public int bookDir = 0;

    [SerializeField] BaseObject[] bookObjects;

    [HideInInspector] public BookView view;
    IBeforeAfterFlip beforeAfterFlip;
    PlayerController player;
    FlipTriggerController flipController;

    public virtual void Awake()
    {
        //開発用。スタートページを設定できる。すごい！
        currentPage = model.setStartPage - 1;

        //BookのView, BookAfterFlipを参照
        view = GetComponent<BookView>();
        beforeAfterFlip = GetComponent<IBeforeAfterFlip>();

        //Controllerを参照
        player = FindFirstObjectByType<PlayerController>();
        flipController = FindFirstObjectByType<FlipTriggerController>();

        //PageのBoneを参照
        leftBones = leftBone.GetComponentsInChildren<Transform>();
        rightBones = rightBone.GetComponentsInChildren<Transform>();
        currentBones = currentBone.GetComponentsInChildren<Transform>();

        //本のオブジェクトを保管する場所を参照
        objectParents = new Transform[objectParent.childCount];
        for (int i = 0; i < objectParent.childCount; i++)
        {
            objectParents[i] = objectParent.GetChild(i);
        }

        //Shapeオブジェクトを参照
        shapes = shape.GetComponentsInChildren<Transform>().Where(w => w != shape).ToArray();

        //本の上のオブジェクトを全て参照
        bookObjects = objectParent.GetComponentsInChildren<BaseObject>();

        //本の表示状態、初期位置を調整
        view.SetAllBookVisibility(true);
        view.SetAllPageVisibility(false);
        view.MoveBookPosition(new Vector3(-5, 0, 0), 0);
    }

    /// <summary>
    /// シーン上のオブジェクトを使用可能にするための工程
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        //左にあるオブジェクトを固定し、右にめめくる
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
        if (isFlipping) { flipTime = Time.time - cTime; }
    }

    /// <summary>
    /// ページ移動
    /// </summary>
    public void Flip(out int currentPagee)
    {
        if (isFlipping)
        {
            currentPagee = 0;
            return;
        }
        else
        {
            currentPagee = 0;
        }

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

        //view.SetPageVisibility(true, false);

        view.SetPageVisibility(2, true);
        view.SetPageVisibility(3, false);

        StartShape();
        foreach (var obj in bookObjects)
        {
            obj.SetBookObject(currentPage, currentBones, shapes, model);
        }

        yield return null;

        view.PlayPageAnimation(2, "Flip");
        view.PlayPageAnimation(3, "Reverse");

        yield return new WaitForSeconds(1.25f);

        //view.SetPageVisibility(false, true);
        view.SetPageVisibility(2, false);
        view.SetPageVisibility(3, true);

        view.SetPageMaterial(currentPage);

        yield return new WaitForSeconds(1.75f);
        yield return new WaitForSeconds(1.75f);
        //yield return new WaitForSeconds(model.flipDelay[currentPage]);

        //
        foreach (var obj in bookObjects)
        {
            obj.AfterFlip(objectParents);
        }
        
        //view.PlayPageAnimation(2, "Reset");
        isFlipping = false;

        //Shapeの位置を初期化
        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].DOPause();
            shapes[i].position = new Vector3(shapes[i].position.x, i < 9 ? 0 : 1, shapes[i].position.z);
        }

        //view.SetPageVisibility(false, false);
        view.SetPageVisibility(2, false);
        view.SetPageVisibility(3, false);

        //進行ができない状態にする
        flipController.SetCanProceed(false);

        //Flipの後のイベントを発生させる
        beforeAfterFlip.OnAfterFlip(currentPage);
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

    public void GameStart()
    {
        StartCoroutine(StartPage());
    }
    IEnumerator StartPage()
    {
        view.MoveBookPosition(Vector3.zero, 2f);

        view.PlayBookAnimation(0, "Open");
        view.PlayBookAnimation(1, "Open");

        yield return new WaitForSeconds(3);
        view.SetPageVisibility(1, true);

        yield return new WaitForSeconds(3);
        view.SetPageVisibility(0, true);

    }

    public void Ending()
    {
        StartCoroutine(EndPage());
    }
    IEnumerator EndPage()
    {
        player.Ending();

        view.SetPageVisibility(2, true);
        view.SetPageVisibility(1, false);
        //view.SetPageVisibility(0, false);

        view.PlayPageAnimation(2, "Flip");
        view.PlayPageAnimation(3, "Reverse");

        yield return new WaitForSeconds(1.25f);

        view.SetPageVisibility(2, false);
        view.SetPageVisibility(3, true);

        yield return new WaitForSeconds(1.25f);
        view.PlayBookAnimation(0, "Close");
        view.PlayBookAnimation(1, "Close");

        //yield return new WaitForSeconds(0.25f);
        view.SetPageVisibility(0, false);
        view.SetPageVisibility(3, false);


        //view.PlayBookAnimation(0, "Close");
        //view.PlayBookAnimation(1, "Close");

        yield return new WaitForSeconds(1);

        view.MoveBookPosition(new Vector3(5, 0, 0), 2f);
    }
}
