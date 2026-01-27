using DG.Tweening;
using System.Collections;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BookController_V3 : MonoBehaviour, IBookController
{
    [Space(20f)]
    [SerializeField] [Range(0, 3)] int currentScene;
    [SerializeField] BookModel_V3 model;
    [SerializeField] BookDelay bookDelay;
    [SerializeField] CinemachineCamera cursorCam;
    //[SerializeField] Image[] turnUI;

    [Space(10f)]
    [SerializeField] Transform objectParent;
    [SerializeField] Transform[] bones, shapes;
    [SerializeField] Distortion[] distortions;

    Transform[] pageL, pageR, pageLC, pageRC, shapeAct, shapeDeact, objectParents;

    [Space(10f)]
    [SerializeField] int currentPage;
    [SerializeField] int bookDirection;

    [SerializeField] float flipTime; //確認用
    bool isFlipping;
    float cTime;

    [SerializeField] UnityEvent onTurnBook, afterTurnBook;
    //pullableObject_V3 에서만 참조.
    public int bookDir;
    bool isBookTurning;

    BookView_V3 view;
    IBookAfterFlip afterFlip;

    IPlayerController playerController;
    IFlipController flipController;

    IBookObject[] bookObjects;
    [SerializeField] MonoBehaviour[] objects;

    IBookDirectional[] directionals;
    [SerializeField] MonoBehaviour[] directionalObjects;

    [SerializeField] UnityEvent onOpen, onStart, onEnd, onCompleted, onBook;


    #region STARTーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
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

        //本の上のオブジェクトを全て参照
        bookObjects = objectParent.GetComponentsInChildren<IBookObject>();
        //ボーンを渡し、ついでに確認用のMonoBehaviour配列を作る
        objects = new MonoBehaviour[bookObjects.Length];
        for (int i = 0; i < bookObjects.Length; i++)
        {
            bookObjects[i].GetBones(pageL, pageR, pageLC, pageRC, shapeAct, shapeDeact);
            objects[i] = bookObjects[i] as MonoBehaviour;
        }

        //Directionalを全て参照
        directionals = FindObjectsByType<MonoBehaviour>
            (FindObjectsInactive.Include, FindObjectsSortMode.None).OfType<IBookDirectional>().ToArray();
        directionalObjects = new MonoBehaviour[directionals.Length];
        for (int i = 0; i < directionals.Length; i++)
        {
            directionalObjects[i] = directionals[i] as MonoBehaviour;
        }

        //全ての歪みを縮小する
        foreach (var obj in distortions)
        {
            if(obj != null)
            {
                obj.transform.localScale = Vector3.zero;
            }
        }

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
        foreach (var obj in bookObjects)
        {
            obj.SetStartParent();
        }

        yield return new WaitForSeconds(0.2f);

        //アニメーションの速度を速くして右にめくる
        view.SetAnimationSpeed(0, 20f);
        view.PlayPageAnimation(0, "Flip");
        view.SetAnimationSpeed(2, 20f);
        view.PlayPageAnimation(2, "Flip");

        yield return new WaitForSeconds(0.3f);

        //固定したオブジェクトをリセット。全てリセットした
        foreach (var obj in bookObjects)
        {
            obj.ResetParent(objectParents);
        }

        yield return new WaitForSeconds(0.1f);

        //アニメーションも元の状態に戻す
        view.PlayPageAnimation(0, "Reset");

        yield return new WaitForSeconds(0.3f);

        //アニメーションの速度も戻す
        view.SetAnimationSpeed(0, 1f);
        view.SetAnimationSpeed(2, 1f);

        //プレイヤーがゲームをスタートできる状態にする
        playerController.SetCanGameStart(pageL, pageR, pageLC, pageRC);
        Debug.Log("Book Start");
    }
    #endregion

    void Update()
    {
        //開発用。ページがめくられる時間を確認できる
        if (isFlipping) { flipTime = Time.time - cTime; }

        /*
        if(pageL != null)
        {
            foreach(var p in pageL)
            {
                if (!p.gameObject.activeSelf) { return; }
                Debug.DrawRay(p.transform.position, p.transform.up, Color.green);
                Debug.DrawRay(p.transform.position, p.transform.right, Color.red);
                Debug.DrawRay(p.transform.position, p.transform.forward, Color.blue);
            }
            foreach(var p in pageR)
            {
                if (!p.gameObject.activeSelf) { return; }
                Debug.DrawRay(p.transform.position, p.transform.up, Color.green);
                Debug.DrawRay(p.transform.position, p.transform.right, Color.red);
                Debug.DrawRay(p.transform.position, p.transform.forward, Color.blue);
            }
            foreach (var p in pageLC)
            {
                if (!p.gameObject.activeSelf) { return; }
               // Debug.DrawRay(p.transform.position, p.transform.up, Color.green);
                //Debug.DrawRay(p.transform.position, p.transform.right, Color.red);
                //Debug.DrawRay(p.transform.position, p.transform.forward, Color.blue);
            }
            foreach (var p in pageRC)
            {
                if (!p.gameObject.activeSelf) { return; }
                Debug.DrawRay(p.transform.position, p.transform.up, Color.green);
                Debug.DrawRay(p.transform.position, p.transform.right, Color.red);
                Debug.DrawRay(p.transform.position, p.transform.forward, Color.blue);
            }
        }
        */
    }

    #region FLIP ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
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
        DisOff(currentPage - 1);

        //Flipの前のイベントを発生させる
        afterFlip.OnBeforeFlip(currentPage);

        //進行ができない状態にする
        flipController.SetCanProceed(false);

        //cursorCam.gameObject.SetActive(true);\

        playerController.SetZoomValue(45);

        //キャラクターを閉じる
        playerController.PlayerFlip(false, currentPage);

        foreach(var obj in bookObjects)
        {
            //今回Flipするオブジェクトを設定
            obj.SetBookObject(currentPage);

            //古いオブジェクトを先に閉じる
            obj.FlipMotion(model, false);
        }

        StartShape(false);

        //前Delay
        yield return new WaitForSeconds(bookDelay.delay[currentPage].x);　//ーーーーーー

        //StartShape(true);

        foreach (var obj in bookObjects)
        {
            //Heightはページのアニメーションに合わせるから古いのと新しいのどっちも実行
            obj.FlipHeight(model, false);
            obj.FlipHeight(model, true);

            //新しいオブジェクトを開く
            obj.FlipMotion(model, true);
        }

        view.PlayPageAnimation(2, "Reverse");
        view.PlayPageAnimation(3, "Flip");

        view.SetPageMaterial(2, currentPage);
        view.SetPageMaterial(3, currentPage - 1);

        //Flipするページを表示
        view.SetPageVisibility(2, true);
        view.SetPageVisibility(3, true);

        yield return new WaitForSeconds(0.1f);

        onBook.Invoke();

        view.SetPageMaterial(1, currentPage);
        Invoke("CustomPageMat", 2.25f);

        yield return new WaitForSeconds(bookDelay.delay[currentPage].y);

        StartShape(true);

        

        yield return new WaitForSeconds(1.25f); //ーーーーーーーーーーーーーーーーーーー

        //後Delay
       // yield return new WaitForSeconds(bookDelay.delay[currentPage].y);　//ーーーーーー

        //キャラクターを開く
        playerController.PlayerFlip(true, currentPage);

        yield return new WaitForSeconds(1f);　//ーーーーーーーーーーーーーーーーーーー

        //view.SetPageMaterial(0, currentPage);

        //Flip中に歪みを拡張するのだけ拡張
        DistortionOn(true);

        yield return new WaitForSeconds(0.75f);　//ーーーーーーーーーーーーーーーーーーー

        

        //キャラクターのFlipを停止
        playerController.StopFlip();

        //ページ移動が終わった後のオブジェクト
        foreach (var obj in bookObjects)
        {
            obj.AfterFlip(objectParents);
        }
        foreach (IBookDirectional d in directionals)
        {
            d?.OnCheckDirectional(bookDir + 2);
        }

        ResetShape();

        //Flipするページを非表示
        view.SetPageVisibility(2, false);
        view.SetPageVisibility(3, false);
        //最初のFlipの後、PageLを表示する
        view.SetPageVisibility(0, true);

        //ページのアニメーションをリセット
        view.PlayPageAnimation(2, "Reset");
        view.PlayPageAnimation(3, "Reset");

        //Flipをしていない状態に設定する
        isFlipping = false;

        //Flipの後のイベントを発生させる
        afterFlip.OnAfterFlip(currentPage);

        //cursorCam.gameObject.SetActive(false);

    }

    void CustomPageMat()
    {
        view.SetPageMaterial(0, currentPage);
    }
    #endregion

    /// <summary>
    /// Shapeを実行。
    /// </summary>
    /// 
    void StartShape(bool isActivate)
    {
        if(isActivate)
        {
            for (int i = 0; i < shapeAct.Length; i++)
            {
                float time = model.curveShape[0].Evaluate(i);
                float delay = model.curveShape[2].Evaluate(i);
                shapeAct[i].DOLocalMoveY(1, time).SetDelay(delay).SetEase(model.easeShape[0]);
            }
        }
        else
        {
            for (int i = 0; i < shapeDeact.Length; i++)
            {
                float time = model.curveShape[1].Evaluate(i);
                float delay = model.curveShape[3].Evaluate(i);
                shapeDeact[i].DOLocalMoveY(0, time).SetDelay(delay).SetEase(model.easeShape[1]);
            }
        }
    }

    /// <summary>
    /// 全てのShapeをリセット
    /// </summary>
    void ResetShape()
    {
        foreach(Transform t in shapeAct)
        {
            t.DOPause();
            t.DOLocalMoveY(0, 0);
        }
        foreach (Transform t in shapeDeact)
        {
            t.DOPause();
            t.DOLocalMoveY(1, 0);
        }
    }


    public void DistortionOn(bool isFlip)
    {
        if(distortions[currentPage] == null) { return; }

        Vector3 scale = Vector3.one * model.distortionValue;
        float time = model.distortionTime;
        if(isFlip)
        {
            distortions[currentPage]?.OnActivateFlip(scale, time, model.easeDistortion);
        }
        else
        {
            distortions[currentPage].OnActivate(scale, time, model.easeDistortion);
        }
    }
    public void DistortionOff()
    {
        Vector3 scale = Vector3.zero;
        float time = model.distortionTime;
        distortions[currentPage]?.OnActivate(scale, time, model.easeDistortion);
    }
    void DisOff(int i)
    {
        Vector3 scale = Vector3.zero;
        float time = model.distortionTime;
        distortions[i]?.OnActivate(scale, time, model.easeDistortion);
    }



    public void TurnBook(bool isRightTurn)
    {
        if ((isRightTurn && bookDir == 2) || (!isRightTurn && bookDir == -2)) { return; }
        if(isBookTurning) { return; }
        isBookTurning = true;

        bookDir = isRightTurn ? bookDir + 1 : bookDir - 1;

        float rot = isRightTurn ? model.rotValue : -model.rotValue;
        view.TurnBookAnimation(rot, model.rotTime);

        LockObjects(true);
        playerController.LockPlayer(true, pageL, pageR);

        onTurnBook.Invoke();
        Invoke("AfterTurnBook", model.rotTime);

        foreach(IBookDirectional d in directionals)
        {
            d.OnCheckDirectional(bookDir + 2);
        }
    }

    void AfterTurnBook()
    {
        flipController.CheckIsBookHorizontal(bookDir);
        isBookTurning = false;

        afterTurnBook.Invoke();
        Invoke("LockFalse", 0.1f);
    }

    void LockFalse()
    {
        playerController.LockPlayer(false, pageL, pageR);
        LockObjects(false);
    }

    void LockObjects(bool onLock)
    {
        foreach (var b in bookObjects)
        {
            b.LockObject(onLock, bookDir);
        }
        distortions[currentPage]?.LockObject(onLock, bookDir);
    }


    public void Ending()
    {
        Debug.Log("asdasdasdsadsad");
        GameStart(false);
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
        onOpen.Invoke();

        view.MoveBookPosition(Vector3.zero, 2f);

        view.PlayBookAnimation(0, "Open");
        view.PlayBookAnimation(1, "Open");

        view.MovePagePosition(1, new Vector3(0, -0.5f, 0), 0f, Ease.Linear);
        view.MovePagePosition(2, new Vector3(0, -0.5f, 0), 0f, Ease.Linear);
        view.MovePagePosition(3, new Vector3(0, -0.5f, 0), 0f, Ease.Linear);

        yield return new WaitForSeconds(0.5f);

        view.MovePagePosition(1, Vector3.zero, 1.0f, Ease.Linear);
        view.MovePagePosition(2, Vector3.zero, 1.0f, Ease.Linear);
        view.MovePagePosition(3, Vector3.zero, 1.0f, Ease.Linear);

        yield return new WaitForSeconds(0.5f);

        //view.SetPageVisibility(1, true);
        view.SetPageVisibility(2, true);
        view.SetPageVisibility(3, true);

        yield return new WaitForSeconds(0.5f);

        view.MovePagePosition(1, Vector3.zero, 0f, Ease.Linear);
        view.MovePagePosition(2, Vector3.zero, 0f, Ease.Linear);
        view.MovePagePosition(3, Vector3.zero, 0f, Ease.Linear);

        Flip();

        yield return new WaitForSeconds(0.1f);
        view.SetPageVisibility(1, true);

        yield return new WaitForSeconds(3f);

        onStart.Invoke();
    }
    IEnumerator EndPage()
    {
        //turnUI[0].DOFade(0, 1);
        //turnUI[1].DOFade(0, 1);
        onEnd.Invoke();

        view.MoveBookPosition(new Vector3(5, 0, 0), 2f);

        Flip();

        view.PlayBookAnimation(0, "Close");
        view.PlayBookAnimation(1, "Close");

        view.SetPageVisibility(1, false);

        view.MovePagePosition(0, new Vector3(0, 0.2f, 0), 3.0f, Ease.Linear);

        yield return new WaitForSeconds(2.2f);

        view.SetPageVisibility(0, false);

        if(currentScene == 0)
        {
            yield return new WaitForSeconds(2f);
            FadeManager.Instance.FadeOut();

            yield return new WaitForSeconds(0.5f);

            GameManager.Instance.SetCanPlay(currentScene);
            GameManager.Instance.ChangeScene(0);
        }
        else
        {
            //여기 마저 해야됨
            yield return new WaitForSeconds(1f);
            //FadeManager.Instance.FadeOut();

            view.SetAllPageVisibility(false);

            view.PlayBookAnimation(2, "End");

            yield return new WaitForSeconds(2f);

            FadeManager.Instance.FadeOut();

            yield return new WaitForSeconds(0.5f);

            GameManager.Instance.SetCanPlay(currentScene);
            GameManager.Instance.ChangeScene(0);

            /*
            yield return new WaitForSeconds(0.8f);

            //--------------------------------------------------
            view.TurnBookRotation(new Vector3(0, 0, -180), 2);

            transform.DOMoveY(2, 1).SetEase(Ease.InCubic);

            yield return new WaitForSeconds(0.5f);
            transform.DOMoveX(-5, 1.5f).SetEase(Ease.OutCubic);

            yield return new WaitForSeconds(3f);
            FadeManager.Instance.FadeOut();

            yield return new WaitForSeconds(0.5f);

            GameManager.Instance.SetCanPlay(currentScene);
            GameManager.Instance.ChangeScene(0);
            */
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (pageL != null)
        {
            foreach (Transform t in pageL)
            {
                Gizmos.DrawSphere(t.position, 0.05f);
                //Gizmos.DrawLine(t.transform.position, t.transform.right);

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
}
