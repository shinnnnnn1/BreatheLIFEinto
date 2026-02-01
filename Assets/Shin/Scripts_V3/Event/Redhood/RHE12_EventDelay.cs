using UnityEngine;
using UnityEngine.Events;

public class RHE12_EventDelay : MonoBehaviour
{
    [SerializeField] UnityEvent ev;
    [SerializeField] float delay;

    public void _StartEvent()
    {
        Invoke("StartEvent", delay);
    }

    void StartEvent()
    {
        ev.Invoke();
    }
}
