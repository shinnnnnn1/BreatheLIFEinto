using UnityEngine;

public class BookView : MonoBehaviour
{
    [SerializeField] Animator[] pageAnim;
    [SerializeField] Animator[] bookAnim;

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
}
