using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class BookController_V3 : MonoBehaviour, IBookController
{
    [Space(20f)]
    [SerializeField] BookModel_V3 model;
    [SerializeField] Transform[] bones, shapes;
    [SerializeField] Transform objectParent;

    //[SerializeField] 
    Transform[] pageL, pageR, pageLC, pageRC, shapeAct, shapeDeact, objectParents;

    [Space(20f)]
    [SerializeField] int currentPage;
    [SerializeField] int bookDirection;

    [SerializeField] float flipTime; //確認用
    bool isFlipping;
    float cTime;

    BookView_V3 view;
    IBookAfterFlip afterFlip;

    IPlayerController playerController;
    IFlipController flipController;

    IBookObject[] bookObjectss;
    
    
    
    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    Transform leftBone, rightBone, currentBone, objectParentt, shape;
    //shape[0 ~ 9] = Activate, shape[10 ~ 18] = Deactivate
    [HideInInspector]
    public Transform[] leftBones, rightBones, currentBones, objectParentss, shapess;

    int bookDir = 0;

    BaseObject[] bookObjects;

    IBeforeAfterFlip beforeAfterFlip;
    PlayerController player;
    //FlipTriggerController flipController;

    void Awake()
    {
        //開発用。スタートページを設定できる
        currentPage = model.setStartPage - 1;

        //参照
        view = GetComponent<BookView_V3>();
        afterFlip = GetComponent<IBookAfterFlip>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayerController>();
        flipController = GameObject.FindGameObjectWithTag("FlipController").GetComponent<IFlipController>();

        //ページのBoneを参照
        pageL = bones[0].GetComponentsInChildren<Transform>();
        pageR = bones[1].GetComponentsInChildren<Transform>();
        pageLC = bones[2].GetComponentsInChildren<Transform>();
        pageRC = bones[3].GetComponentsInChildren<Transform>();

        //Shapeを参照
        shapeAct = shapes[0].GetComponentsInChildren<Transform>().Where(w => w != shapes[0]).ToArray();
        shapeDeact = shapes[1].GetComponentsInChildren<Transform>().Where(w => w != shapes[1]).ToArray();

        //本のオブジェクトを保管する場所を参照
        objectParents = new Transform[objectParent.childCount];
        for (int i = 0; i < objectParent.childCount; i++) { objectParents[i] = objectParent.GetChild(i); }

        //本の表示状態、初期位置を設定
        view.SetAllBookVisibility(true);
        view.SetAllPageVisibility(false);
        view.MoveBookPosition(new Vector3(-5, 0, 0), 0);
    }

    //全てのオブジェクトを使用可能な状態にする
    IEnumerator Start()
    {
        //全てのオブジェクトが準備するまで大気
        yield return new WaitForSeconds(0.1f);

        //左にあるオブジェクトを固定し、右にめくる
        /*
        foreach (var obj in bookObjects)
        {
            if (!obj.isRight)
            {
                obj.SetParentStart();
            }
        }
        */

        yield return new WaitForSeconds(0.2f);

        //アニメーションの速度を速くして右にめくる
        view.SetAnimationSpeed(0, 20f);
        view.PlayPageAnimation(0, "Flip");
        view.SetAnimationSpeed(2, 20f);
        view.PlayPageAnimation(2, "Flip");

        yield return new WaitForSeconds(0.3f);

        //固定したオブジェクトをリセット
        /*
        foreach (var obj in bookObjects)
        {
            if (!obj.isRight)
            {
                obj.ResetParent(objectParents);
            }
        }
        */

        yield return new WaitForSeconds(0.1f);

        //アニメーションも元の状態に戻す
        view.PlayPageAnimation(0, "Reset");

        yield return new WaitForSeconds(0.3f);

        //アニメーションの速度も戻す
        view.SetAnimationSpeed(0, 1f);
        view.SetAnimationSpeed(2, 1f);

        //プレイヤーがゲームをスタートできる状態にする
        playerController?.SetCanGameStart(pageL, pageR, pageLC, pageRC);
        Debug.Log("Can");
    }

    void Update()
    {
        //開発用。ページがめくられる時間を確認できる
        if (isFlipping) { flipTime = Time.time - cTime; }
    }

    /// <summary>
    /// ページ移動
    /// </summary>
    public void Flips(out int currentPagee)
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

    IEnumerator FlipCoroutinee()
    {
        beforeAfterFlip.OnBeforeFlip(currentPage, out int beforeWaitTime);
        yield return new WaitForSeconds(beforeWaitTime);

        //view.SetPageVisibility(true, false);

        view.SetPageVisibility(2, true);
        view.SetPageVisibility(3, false);

        StartShape();
        foreach (var obj in bookObjects)
        {
            //obj.SetBookObject(currentPage, currentBones, shapes, model);
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
        //flipController.SetCanProceed(false);

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

        if ((isRightTurn && bookDir == 1) || (!isRightTurn && bookDir == -1)) { canTurn = false; return; }
        canTurn = true;

        bookDir = isRightTurn ? bookDir + 1 : bookDir - 1;

        float rot = isRightTurn ? model.rotValue : -model.rotValue;
        view.TurnBookAnimation(rot, model.rotTime);

        LockObjects(true);

        Invoke("AfterTurnBook", model.rotTime);
    }

    void AfterTurnBook()
    {
        LockObjects(false);
        player.LockPlayer(false);
        //flipController.CheckBookIsHorizontal(bookDir);
    }

    void LockObjects(bool onLock)
    {
        foreach (BaseObject b in bookObjects)
        {
            if (b.isCurrent)
            {
                b.isLocked = onLock;
                if (!onLock)
                {
                    NPCObject n = b.GetComponent<NPCObject>();
                    n?.CheckDirectional(bookDir);
                }
            }
        }
    }

    #region FLIP ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    /// <summary>
    /// ページをめくる
    /// </summary>
    /// <seealso cref="PlayerController_V3.Update()"/>
    /// <seealso cref="GameStart(bool)"/>
    public void Flip()
    {
        isFlipping = true;
        cTime = Time.time;

        currentPage++;
        Debug.Log($"Currently on Page [ {currentPage} ]");

        StartCoroutine(FlipCoroutine());
    }

    IEnumerator FlipCoroutine()
    {

        playerController.PlayerFlip(false, currentPage);

        //前Delay
        yield return new WaitForSeconds(0);

        view.PlayPageAnimation(2, "Reverse");
        view.PlayPageAnimation(3, "Flip");

        yield return new WaitForSeconds(1.25f);

        //後Delay
        yield return new WaitForSeconds(0);

        playerController.PlayerFlip(true, currentPage);
        yield return new WaitForSeconds(1.75f);

        //
        playerController.StopFlip();

        //
        view.PlayPageAnimation(2, "Reset");
        view.PlayPageAnimation(3, "Reset");

        //
        isFlipping = false;

        //進行ができない状態にする
        //flipController.

        //Flipの後のイベントを発生させる
        //afterFlip.OnAfterFlip(currentPage);
    }

    void DeactivateObjects(bool isActivate)
    {

    }
    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (pageL != null)
        {
            foreach (Transform t in pageL)
            {
                Gizmos.DrawSphere(t.position, 0.05f);
            }
            foreach (Transform t in pageR)
            {
                Gizmos.DrawSphere(t.position, 0.05f);
            }
            foreach (Transform t in pageRC)
            {
                Gizmos.DrawSphere(t.position, 0.05f);
            }
        }
    }

    /// <summary>
    /// 最初と最後の本の動き
    /// </summary>
    /// <seealso cref="PlayerController_V3.IAnyKey()"/>
    public void GameStart(bool isStart)
    {
        StartCoroutine(isStart ? StartPage() : EndPage());
    }
    IEnumerator StartPage()
    {
        view.MoveBookPosition(Vector3.zero, 2f);

        view.PlayBookAnimation(0, "Open");
        view.PlayBookAnimation(1, "Open");

        view.MovePagePosition(1, new Vector3(0, -0.5f, 0), 0f);
        view.MovePagePosition(2, new Vector3(0, -0.5f, 0), 0f);
        view.MovePagePosition(3, new Vector3(0, -0.5f, 0), 0f);

        yield return new WaitForSeconds(1.5f);

        Flip();

        view.MovePagePosition(1, Vector3.zero, 2f);
        view.MovePagePosition(2, Vector3.zero, 2f);
        view.MovePagePosition(3, Vector3.zero, 2f);

        //Flip()에서 하는것들은 지워도 될듯
        view.SetPageVisibility(1, true);
        view.SetPageVisibility(2, true);
        view.SetPageVisibility(3, true);


        yield return new WaitForSeconds(1.25f);

        yield return new WaitForSeconds(1.25f);
        view.SetPageVisibility(0, true);
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
