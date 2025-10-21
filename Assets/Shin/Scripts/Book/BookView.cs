using UnityEngine;
using DG.Tweening;

public class BookView : MonoBehaviour
{
    [SerializeField] Animator[] pageAnim;
    [SerializeField] Animator[] bookAnim;
    [SerializeField] SkinnedMeshRenderer[] pageMesh;

    public void MoveBookPosition(Vector3 pos, float duration)
    {
        transform.DOMove(pos, duration).SetEase(Ease.OutQuint);
    }

    public void PlayBookAnimation(int pageNum, string trigger)
    {
        bookAnim[pageNum].SetTrigger(trigger);
    }

    public void PlayPageAnimation(int pageNum, string trigger)
    {
        pageAnim[pageNum].SetTrigger(trigger);
    }
    
    public void SetAnimationSpeed(int pageNum, float speed)
    {
        pageAnim[pageNum].speed = speed;
    }

    public void SetAllPageVisibility(bool isVisible)
    {
        foreach(var page in pageMesh)
        {
            page.gameObject.SetActive(isVisible);
        }
    }

    public void SetPageVisibility(int pageNum, bool isVisible)
    {
        pageMesh[pageNum].gameObject.SetActive(isVisible);
    }

    public void SetPageVisibility(bool right, bool left)
    {
        pageMesh[2].gameObject.SetActive(right);
        pageMesh[3].gameObject.SetActive(left);
    }

    public void SetPageMaterial(int currentPage)
    {

    }

    public void TurnBookAnimation(bool isRightTurn, float rotValue, float rotTime)
    {
        transform.DORotate(new Vector3(0, rotValue, 0), rotTime).SetRelative().SetEase(Ease.Linear);
    }
}
