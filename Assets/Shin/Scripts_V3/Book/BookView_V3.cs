using UnityEngine;
using DG.Tweening;


public class BookView_V3 : MonoBehaviour
{
    [SerializeField] Animator[] bookAnim;
    [SerializeField] Animator[] pageAnim;
    [SerializeField] SkinnedMeshRenderer[] pageMesh;

    //全ての本の表示状態を設定
    public void SetAllBookVisibility(bool isVisible)
    {
        foreach (var book in bookAnim)
        {
            book.gameObject.SetActive(isVisible);
        }
    }
    //全てのページの表示状態を設定
    public void SetAllPageVisibility(bool isVisible)
    {
        foreach (var page in pageMesh)
        {
            page.gameObject.SetActive(isVisible);
        }
    }
    //一つのページの表示状態を設定
    public void SetPageVisibility(int pageNum, bool isVisible)
    {
        pageMesh[pageNum].gameObject.SetActive(isVisible);
    }

    //本のアニメーションを再生
    public void PlayBookAnimation(int pageNum, string trigger)
    {
        bookAnim[pageNum].SetTrigger(trigger);
    }
    //ページのアニメーションを再生
    public void PlayPageAnimation(int pageNum, string trigger)
    {
        pageAnim[pageNum].SetTrigger(trigger);
    }
    //ページのアニメーション速度を設定
    public void SetAnimationSpeed(int pageNum, float speed)
    {
        pageAnim[pageNum].speed = speed;
    }

    //本を動かせる
    public void MoveBookPosition(Vector3 pos, float duration)
    {
        transform.DOMove(pos, duration).SetEase(Ease.OutQuint);
    }
    //本を回す
    public void TurnBookAnimation(float rotValue, float rotTime)
    {
        transform.DORotate(new Vector3(0, rotValue, 0), rotTime)
            .SetRelative().SetEase(Ease.Linear);
    }

    public void MovePagePosition(int pageNum, Vector3 pos, float duration)
    {
        pageAnim[pageNum].transform.DOLocalMove(pos, duration).SetEase(Ease.OutQuint);
    }

    //マテリアル。。。
    public void SetPageMaterial(int currentPage)
    {

    }
}
