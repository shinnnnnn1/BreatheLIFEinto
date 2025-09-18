using UnityEngine;

public class BookAfterFlip : MonoBehaviour
{
    PlayerController player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    public void AfterFlip(int currentStage)
    {
        switch (currentStage)
        {
            case 1:
                //EventManager.Instance.PlayCutScene(0);
                break;
            case 2:
                player.SetCanMove(true);
                break;
            case 3:
                //EventManager.Instance.PlayCutScene(2);
                break;
            case 4:
                //EventManager.Instance.PlayCutScene(2);
                break;
            case 5:
                player.SetCanMove(true);
                break;
            case 6:
                player.SetCanMove(true);
                break;
            

        }

    }
}
