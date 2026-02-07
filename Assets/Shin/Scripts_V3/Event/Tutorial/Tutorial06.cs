using UnityEngine;
using UnityEngine.Events;

public class Tutorial06 : TutorialEvent
{
    [Space(10f)]
    [SerializeField] bool isTape, isHorizontal;
    [SerializeField] BookController_V3 controller;
    [SerializeField] UnityEvent on1, on0;

    public void _SetIsTape()
    {
        isTape = true;
    }

    public void _CheckIsHorizontal()
    {
        if (!canPlay) { return; }

        if(controller.bookDir == 0)
        {
            if(isTape)
            {
                isHorizontal = true;
            }
            else
            {
                Invoke("Invoke0", 1);
            }
        }
        else if (controller.bookDir == 1 && !isTape)
        {
            on1.Invoke();
        }
    }
    void Invoke0()
    {
        on0.Invoke();
    }

    public override bool QuestComplete()
    {
        if (isTape && isHorizontal)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
