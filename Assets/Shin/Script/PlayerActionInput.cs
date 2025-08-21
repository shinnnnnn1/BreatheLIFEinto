using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionInput : MonoBehaviour
{
    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void InputMove(InputAction.CallbackContext context)
    {
        controller.inputDirection = context.ReadValue<Vector2>();
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.Jump();
        }
    }
    public void InputAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Action(false);
        }
        else if (context.canceled)
        {
            //Action(false);
        }
    }
    public void InputZoom(InputAction.CallbackContext context)
    {

    }
    public void InputSwitchCharacterToPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }
    public void InputPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
        }
    }

}
