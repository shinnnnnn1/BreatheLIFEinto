using UnityEngine;

public class RedhoodBeforeAfterFlip : MonoBehaviour, IBeforeAfterFlip
{
    PlayerController player;
    FlipTriggerController trigger;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        trigger = FindFirstObjectByType<FlipTriggerController>();
    }

    public void OnBeforeFlip(int currentStage, out int waitTime)
    {
        waitTime = 0;
        switch (currentStage)
        {
            default:
                //Debug.Log("Defalut");
                break;
                
            case 7:

                break;
            case 8:

                break;
        }
    }

    public void OnAfterFlip(int currentStage)
    {
        switch (currentStage)
        {
            case 1:
                EventManager.Instance.PlayCutScene(0);
                break;
            case 2:
                player.SetCanMove(true);
                break;
            case 3:
                player.SetCanMove(true);
                EventManager.Instance.PlayCutScene(2);
                break;
            case 4:
                //EventManager.Instance.PlayCutScene(2);
                break;
            case 5:
                
                break;
            case 6:
                
                break;
            case 7:
                trigger.ResetTrigger(1);

                break;
            case 8:
                trigger.ResetTrigger(2);
                break;
        }
    }
}
