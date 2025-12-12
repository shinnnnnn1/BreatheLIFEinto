using UnityEngine;

public class Tutorial04 : TutorialEvent
{
    [Space(10f)]
    [SerializeField] bool isSwitch;
    [SerializeField] PlayerController_V3 playerController;
    

    public override bool QuestComplete()
    {
        if ((isSwitch  && playerController.zoomDirection.x < 0) || (!isSwitch && playerController.zoomDirection.x > 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
