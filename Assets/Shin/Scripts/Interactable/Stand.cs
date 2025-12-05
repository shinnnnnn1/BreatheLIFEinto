using UnityEngine;
using UnityEngine.Events;

public class Stand : MonoBehaviour, ICursorInteractable
{
    public bool canActivate, isEntered, canSwitch, isActivated;
    [SerializeField] UnityEvent onEnter, onExit, 
        onPressedA, onReleasedA, onPressedDea, onReleasedDea, onCanceled;

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
            Debug.Log("PressedD");
            onPressedDea.Invoke();
        }
        else
        {
            Debug.Log("PressedA");
            onPressedA.Invoke();
        }
    }
    public virtual void OnReleased()
    {
        if (!canActivate) { return; }

        Debug.Log("Released");

        if (isActivated)
        {
            onReleasedDea.Invoke();
        }
        else
        {
            onReleasedA.Invoke();
        }

        if(canSwitch)
        {
            isActivated = !isActivated;
        }
    }

    public void OnCanceled()
    {
        onCanceled.Invoke();
    }



    public void SetCanActivate(bool canA)
    {
        canActivate = canA;
    }
    public void SetIsActivated(bool isA)
    {
        isActivated = isA;
    }
}
