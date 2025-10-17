using UnityEngine;

public class FlipTrigger : MonoBehaviour
{
    [HideInInspector] public FlipTriggerController controller;

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
