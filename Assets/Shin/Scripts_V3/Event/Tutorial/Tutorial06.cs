using UnityEngine;

public class Tutorial06 : TutorialEvent
{
    [Space(10f)]
    [SerializeField] bool isTape, isHorizontal;
    [SerializeField] BookController_V3 controller;

    public void _SetIsTape()
    {
        isTape = true;
    }

    public void _CheckIsHorizontal()
    {
        if (!canPlay) { return; }

        if(controller.bookDir == 0 && isTape)
        {
            isHorizontal = true;
        }
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
