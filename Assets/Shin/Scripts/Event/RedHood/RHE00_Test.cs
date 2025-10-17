using UnityEngine;
using UnityEngine.Events;

public class RHE00_Test : MonoBehaviour
{
    [SerializeField] UnityEvent e;

    private void OnEnable()
    {
        e.Invoke();
    }
}
