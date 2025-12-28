using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerActionInput_V3 : MonoBehaviour
{
    PlayerController_V3 controller;
    [SerializeField] UnityEvent deb;

    void Start()
    {
        controller = GetComponent<PlayerController_V3>();
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
        if (context.started)
        {
            Time.timeScale = 30;
        }
        else if (context.canceled)
        {
            Time.timeScale = 1;
        }
    }
    public void InputDebug2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            deb.Invoke();
            //controller.PlayerFlipTrigger();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void InputAnyKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.IAnyKey();
        }
    }

    public void InputTurnBookL(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.TurnBook(false);
        }
    }

    public void InputTurnBookR(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.TurnBook(true);
        }
    }
}
