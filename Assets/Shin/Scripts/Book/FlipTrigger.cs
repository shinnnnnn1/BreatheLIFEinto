using UnityEngine;

public class FlipTrigger : MonoBehaviour
{
    FlipTriggerController controller;

    void Start()
    {
        controller = GetComponentInParent<FlipTriggerController>();
    }

    void OnTriggerStay(Collider other)
    {
        if(controller.CanTrigger())
        {
            controller.playerController.PlayerFlipTrigger();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(controller.CanTrigger())
        {
            controller.playerController.PlayerFlipTrigger();
        }
    }
}
