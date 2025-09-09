using UnityEngine;

public class FlipTrigger : MonoBehaviour
{
    public bool canFlip = true;
    PlayerController playerController;

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    void OnTriggerStay(Collider other)
    {
        playerController.PlayerFlipTrigger();
    }
}
