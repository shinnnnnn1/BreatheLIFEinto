using UnityEngine;
using UnityEngine.Events;

public class RHE70_TouchWolf : MonoBehaviour
{
    [SerializeField] UnityEvent[] events;
    [SerializeField] int i;

    public void _TouchWolf()
    {
        events[i].Invoke();
        i++;
    }
}
