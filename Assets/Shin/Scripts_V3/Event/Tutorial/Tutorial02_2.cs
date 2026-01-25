using UnityEngine;
using UnityEngine.Events;

public class Tutorial02_2 : MonoBehaviour
{
    [SerializeField] bool isSwitch;
    [SerializeField] bool canPlay;
    [SerializeField] bool isCompleted;
    [SerializeField] UnityEvent onComplete;

    [SerializeField] PlayerController_V3 playerController;

    public void _SetTutorialCanPlay()
    {
        canPlay = true;
    }

    void Update()
    {
        if (canPlay)
        {
            if (playerController.IsPulling() && 
                ( isSwitch ? playerController.moveDirection.x < 0 : playerController.moveDirection.x > 0))
            {
                isCompleted = true;
                canPlay = false;
                onComplete.Invoke();
            }
        }
        if(isCompleted)
        {

        }
    }
}
