using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public UnityEvent unityEvent;
    bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!isActivated)
        {
            unityEvent.Invoke();
            isActivated = true;
        }
    }
}
