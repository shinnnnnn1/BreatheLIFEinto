using UnityEngine;
using UnityEngine.Events;

public class Stand : MonoBehaviour, ICursorInteractable
{
    public bool canActivate, isEntered, isActivated;
    [SerializeField] UnityEvent onEnter, onExit, 
        onPressedA, onReleasedA, onPressedDea, onReleasedDea;

    public virtual void OnEnter()
    {
        if (!canActivate) { return; }
        if(isEntered) { return; }

        isEntered = true;
        onEnter.Invoke();
    }
    public virtual void OnExit()
    {
        if (!canActivate) { return; }

        isEntered = false;
        onExit.Invoke();
    }
    public virtual void OnPressed()
    {
        if (!canActivate) { return; }

        if(isActivated)
        {
            onPressedDea.Invoke();
        }
        else
        {
            onPressedA.Invoke();
        }
    }
    public virtual void OnReleased()
    {
        if (!canActivate) { return; }

        if (isActivated)
        {
            onReleasedDea.Invoke();
        }
        else
        {
            onReleasedA.Invoke();
        }

        isActivated = !isActivated;
    }



    public void SetCanActivate(bool canA)
    {
        canActivate = canA;
    }
}
