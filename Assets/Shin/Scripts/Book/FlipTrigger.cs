using UnityEngine;

public class FlipTrigger : MonoBehaviour
{
   public FlipTriggerController controller;

    void Start()
    {
        controller = GetComponentInParent<FlipTriggerController>();
    }

    void OnTriggerStay(Collider other)
    {
        if(controller.canProceed && controller.isBookHorizontal)
        {
            controller.playerController.PlayerFlipTrigger();
        }
    }
}
