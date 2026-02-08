using UnityEngine;
using UnityEngine.Events;

public class RHE84_TurnUI : MonoBehaviour
{
    [SerializeField] bool canCheck = true;
    [SerializeField] int stage;
    [SerializeField] int goal;
    [SerializeField] BookController_V3 book;
    [SerializeField] RHE10_LoopEvent loopL, loopR;
    [SerializeField] UnityEvent onGoal;

    public void _SetCanCheck(bool c) => canCheck = c;
    public void _Check()
    {
        if (book.currentPage != stage || !canCheck) { return; }

        int d = book.bookDir;
        if (d != goal)
        {
            if(d < goal)
            {
                loopL._StopLoop();
                Invoke("StartR", 1);
            }
            else
            {
                loopR._StopLoop();
                Invoke("StartL", 1);
            }
        }
        else
        {
            onGoal.Invoke();
            loopL._StopLoop();
            loopR._StopLoop();
        }
    }

    void StartL()
    {
        loopL._StartLoop();
    }
    void StartR()
    {
        loopR._StartLoop();
    }
}
