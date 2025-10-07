using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public int eventIndex;
    bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!isActivated)
        {
            EventManager.Instance.PlayCutScene(eventIndex);
            isActivated = true;
        }
    }
}
