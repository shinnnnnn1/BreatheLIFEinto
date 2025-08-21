using UnityEngine;
using UnityEngine.InputSystem;

public class CursorInput : MonoBehaviour
{
    CursorController controller;
    private void Start()
    {
        controller = GetComponent<CursorController>();
    }

    public void InputPoint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Vector2 value = context.ReadValue<Vector2>();
            //controller.SetCursorPosition(value);
            //Debug.Log(value);
        }
        Vector2 value = context.ReadValue<Vector2>();
        //controller.SetCursorPosition(value);
    }
    public void InputRelease(InputAction.CallbackContext context)
    {
        if(context.performed)
        {

        }
        else if(context.canceled)
        {

        }
    }
}
