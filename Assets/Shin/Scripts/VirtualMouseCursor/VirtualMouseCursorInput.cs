using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualMouseCursorInput : MonoBehaviour
{
    VirtualMouseController controller;

    void Start()
    {
        controller = GetComponent<VirtualMouseController>();
    }

    public void InputControlsChanged()
    {
        controller?.OnControlsChanged();
    }
    public void InputClick(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
           // controller.SetCursorMode(false);
        }
        else if(context.canceled)
        {

        }
    }

    public void InputActivateCursorMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.SetCursorMode(true);
        }
    }
    public void InputDeactivateCursorMode(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.SetCursorMode(false);
        }
    }


}
