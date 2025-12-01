using UnityEngine;
using UnityEngine.Events;

public class HoldableObject_V3 : MonoBehaviour, IInteractable
{
    [SerializeField] UnityEvent onEnter, onExit;
    [SerializeField] bool isEntered;

    public void OnEnter(bool isRight)
    {
        if (!isEntered)
        {
            onEnter.Invoke();
            isEntered = true;
        }
    }
    public void OnExit()
    {
        onExit.Invoke();
        isEntered = false;
    }
}
