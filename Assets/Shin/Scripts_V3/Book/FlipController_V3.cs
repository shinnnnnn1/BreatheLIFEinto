using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlipController_V3 : MonoBehaviour, IFlipController
{
    [SerializeField] bool canProceed = false;
    [SerializeField] bool isBookHorizontal = true;

    [SerializeField] int currentTrigger = 0;
    [SerializeField] FlipTrigger_V3[] triggers;

    [SerializeField] FadeUI proceedImage;

    public IPlayerController playerController;
    public IBookController bookController;

    void Start()
    {
        //FlipTriggerを参照
        triggers = GetComponentsInChildren<FlipTrigger_V3>();

        //0番のTriggerだけ表示
        ResetTrigger(currentTrigger);

        //Controllerを参照
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<IPlayerController>();
        bookController = GameObject.FindGameObjectWithTag("BookController").GetComponent<IBookController>();
    }

    //AfterFlip UnityEventで使用
    public void ResetTrigger(int currentTrigger)
    {
        Debug.Log("Reset to " + currentTrigger);

        //表示したいTriggerだけ表示
        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].gameObject.SetActive(i == currentTrigger);
        }
    }

    //進行できる状態や水平状態を設定
    public void SetCanProceed(bool can)
    {
        Debug.Log("SetCanProceed " + can);

        //進行できる状態に設定
        canProceed = can;

        //本が水平なのか確認
        //CheckBookIsHorizontal(bookController.bookDir);

        //proceedImage.DOFade(can ? 1 : 0, 1);

        proceedImage.Fade(can ? 1 : 0);
    }

    public void CheckIsBookHorizontal(int bookAngle)
    {
        isBookHorizontal = bookAngle == 0;
    }

    //最終的に進行ができる状態の確認。進行できる + 本が水平 = 最終的に進行できる
    public bool CanTrigger()
    {
        if (canProceed && isBookHorizontal) { return true; }
        else { return false; }
    }



    void OnDrawGizmos()
    {
        if (triggers.Length > 0)
        {
            Gizmos.color = CanTrigger() ? Color.green : Color.red;
            Gizmos.DrawSphere(triggers[0].transform.position, 0.5f);
        }
    }
}
