using UnityEngine;

public class FlipTrigger_V3 : MonoBehaviour
{
    FlipController_V3 controller;

    void Start()
    {
        controller = GetComponentInParent<FlipController_V3>();
    }

    void OnCollisionStay(Collision collision)
    {
        if (controller.CanTrigger())
        {
            controller.playerController.PlayerFlipTrigger();
        }
    }
}
