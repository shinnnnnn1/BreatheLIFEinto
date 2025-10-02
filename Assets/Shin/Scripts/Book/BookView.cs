using UnityEngine;
using DG.Tweening;

public class BookView : MonoBehaviour
{
    [SerializeField] Animator[] pageAnim;
    [SerializeField] Animator[] bookAnim;
    [SerializeField] SkinnedMeshRenderer[] pageMesh;

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
            page.enabled = isVisible;
        }
    }

    public void SetPageVisibility(bool right, bool left)
    {
        pageMesh[2].gameObject.SetActive(right);
        pageMesh[3].gameObject.SetActive(left);
    }

    public void SetPageMaterial(int currentPage)
    {

    }

    public void TurnBookAnimation(bool isRightTurn)
    {

    }
}
