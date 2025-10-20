using UnityEngine;
using UnityEngine.Events;

public class RHE33_CanEnding : MonoBehaviour
{
    [SerializeField] UnityEvent eventt;

    bool isA;

    private void OnTriggerEnter(Collider other)
    {
        if (isA) { return; }
        isA = true;

        eventt.Invoke();
    }
}
