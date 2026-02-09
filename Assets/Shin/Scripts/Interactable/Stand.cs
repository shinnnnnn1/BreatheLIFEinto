using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Stand : BookDirectional, ICursorInteractable
{
    public bool canActivate, isEntered, canSwitch, isActivated;
    [SerializeField] BookController_V3 bookController;
    //[SerializeField] Collider coll;
    [SerializeField] UnityEvent onEnter, onExit, 
        onPressedA, onReleasedA, onPressedDea, onReleasedDea, onCanceled;
    //[SerializeField] bool[] isDirectional = new bool[] { false, false, true, false, false }; // -1, 0, 1


    void Start()
    {
        bookController = FindFirstObjectByType<BookController_V3>();
    }
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
            //Debug.Log("PressedD");
            onPressedDea.Invoke();
        }
        else
        {
            //Debug.Log("PressedA");
            onPressedA.Invoke();
        }
    }
    public virtual void OnReleased()
    {
        if (!canActivate) { return; }

        //Debug.Log("Released");

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

    /*
    public void CheckDirectional()
    {
        if (isDirectional[bookController.bookDir + 2])
        {
            coll.enabled = true;
        }
        else
        {
            coll.enabled = false;
        }
    }
    */
}
