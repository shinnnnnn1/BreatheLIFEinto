using UnityEngine;
using UnityEngine.Events;

public class Stand : MonoBehaviour, ICursorInteractable
{
    public bool canActvate, isEntered, isActivated;
    [SerializeField] UnityEvent onEnter, onExit, onPressed, onReleased;

    public virtual void OnEnter()
    {
        if (!canActvate) { return; }
        if(isEntered) { return; }

        isEntered = true;
        onEnter.Invoke();
    }
    public virtual void OnExit()
    {
        if (!canActvate) { return; }

        isEntered = false;
        onExit.Invoke();
    }
    public virtual void OnPressed()
    {
        if (!canActvate) { return; }

        onPressed.Invoke();
    }
    public virtual void OnReleased()
    {
        if (!canActvate) { return; }

        isActivated = !isActivated;
        onReleased.Invoke();
    }
}
