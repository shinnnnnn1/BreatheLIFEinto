using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionInput : MonoBehaviour
{
    PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        controller.moveDirection = context.ReadValue<Vector2>();
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.Jump();
        }
    }

    public void InputZoom(InputAction.CallbackContext context)
    {
        controller.zoomDirection = context.ReadValue<Vector2>();
    }

    public void InputAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.Action();
        }
        else if (context.canceled)
        {
            controller.ActionCancel();
        }
    }

    public void InputDebug(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Time.timeScale = 30;
        }
        else if (context.canceled)
        {
            Time.timeScale = 1;
        }
    }
    public void InputAnyKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.SetGameStart();
        }
    }
}
