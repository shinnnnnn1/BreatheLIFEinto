using UnityEngine;
using UnityEngine.Events;

public class RedhoodBeforeAfterFlip : MonoBehaviour, IBeforeAfterFlip
{
    PlayerController player;
    FlipTriggerController trigger;


    [SerializeField] UnityEvent[] beforeFlip = new UnityEvent[10];
    [SerializeField] UnityEvent[] afterFlip = new UnityEvent[10];

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
        afterFlip[currentStage].Invoke();
    }
}
