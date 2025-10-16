using UnityEngine;

public class Stand : MonoBehaviour, ICursorInteractable
{
    [SerializeField] bool canActvate, isActivated;

    public void OnEnter()
    {
        Debug.Log("Enter " + gameObject.name);
    }
    public void OnExit()
    {
        Debug.Log("Exit " + gameObject.name);
    }
    public void OnActivate()
    {
        if(isActivated)
        {
            isActivated = false;
        }
        else
        {
            isActivated= true;
        }
    }
    public void OnDeactivate()
    {

    }
}
