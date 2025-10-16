using UnityEngine;
using UnityEngine.Events;

public class CursorInteractableBase : MonoBehaviour, ICursorInteractable
{
    [SerializeField] bool canActivate, isEntered, isActivated = false;
    [SerializeField] UnityEvent onActivated, onDeactivated;

    public void OnEnter()
    {
        if (isEntered) { return; }

        isEntered = true;

    }
    public void OnExit()
    {
        isEntered = false;

    }
    public void OnPressed()
    {

    }
    public void OnReleased()
    {
        isEntered = false;

        isActivated = !isActivated;
        if(isActivated)
        {
            onActivated.Invoke();
        }
        else
        {
            onDeactivated.Invoke();
        }
    }
}
