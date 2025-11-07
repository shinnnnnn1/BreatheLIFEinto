using System.Collections;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class BookController_V3 : MonoBehaviour, IBookController
{
    [Space(20f)]
    [SerializeField] BookModel_V3 model;

    [Space(10f)]
    [SerializeField] Transform objectParent;
    [SerializeField] Transform[] bones, shapes;

    //[SerializeField] 
    Transform[] pageL, pageR, pageLC, pageRC, shapeAct, shapeDeact, objectParents;

    [Space(10f)]
    [SerializeField] int currentPage;
    [SerializeField] int bookDirection;

    [SerializeField] float flipTime; //確認用
    bool isFlipping;
    float cTime;

    BookView_V3 view;
    IBookAfterFlip afterFlip;

    IPlayerController playerController;
    IFlipController flipController;

    IBookObject[] bookObjects;
    [SerializeField] MonoBehaviour[] objects;
    
    

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
            bookObjects[i].GetBones(pageL, pageR, pageLC, pageRC);
            objects[i] = bookObjects[i] as MonoBehaviour;
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
        Debug.Log("Can");
    }

    void Update()
    {
        //開発用。ページがめくられる時間を確認できる
        if (isFlipping) { flipTime = Time.time - cTime; }
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
    //★★★★★★★★★
    //  선딜 후딜 관련해서 애니메이션 후에 나오는 오브젝트 등 일단 대기중.
    //
    //★★★★★★★★★
    IEnumerator FlipCoroutine()
    {
        //キャラクターを閉じる
        playerController.PlayerFlip(false, currentPage);

        //古い、新しいオブジェクトの設定をして、古いオブジェクトを先に閉じる
        foreach(var obj in bookObjects)
        {
            obj.SetBookObject(currentPage, model);
            //obj.FlipDeactivateObject();
        }

        //前Delay
        yield return new WaitForSeconds(0);

        //新しいオブジェクトを開く

        yield return null;

        //LCとRCのアニメーション再生
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
        yield return new WaitForSeconds(1.5f);
    }

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
}
