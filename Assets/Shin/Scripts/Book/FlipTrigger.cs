using UnityEngine;

public class FlipTrigger : MonoBehaviour
{
   public  FlipTriggerController controller;

    void Start()
    {
        controller = GetComponentInParent<FlipTriggerController>();
    }

    void OnTriggerStay(Collider other)
    {
        controller.playerController.PlayerFlipTrigger();
    }
}
