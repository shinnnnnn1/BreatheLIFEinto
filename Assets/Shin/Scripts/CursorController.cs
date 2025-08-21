using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class CursorController : MonoBehaviour
{
    CursorView cursorView;

    [SerializeField] RectTransform canvas;
    [SerializeField] RectTransform cursor;

    PlayerInput playerInput;
    Camera mainCamera;
    Mouse currentMouse;
    Mouse virtualMouse;

    public Vector2 inputDirection;
    public float cursorSpeed = 1000f;
    public float padding = 30f;

    const string gamepadScheme = "Gamepad";
    const string mouseScheme = "Keyboard&Mouse";

    void Awake()
    {
        //Get Reference to Componenets and Input Devices
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        //Ensure a virtual mouse exists and is registered in the InputSystem
        if (virtualMouse == null)
        {
            //If no virtual mouse exists, create and register it
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            //If it exists bus is not yet added to the InputSystem, register it
            InputSystem.AddDevice("VirtualMouse");
        }

        //Pair the virtual mouse with the PlayerInput user
        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        Vector2 position = cursor.anchoredPosition;
        InputState.Change(virtualMouse.position, position);


    }

    void OnDisable()
    {
        if (virtualMouse != null && virtualMouse.added)
        {
            InputSystem.RemoveDevice(virtualMouse);
        }
    }

    void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (canvas, position, mainCamera, out anchoredPosition);
        cursor.anchoredPosition = anchoredPosition;
    }

    public void SetCursorPosition(Vector2 direction)
    {
        if (virtualMouse == null || !virtualMouse.added || Gamepad.current == null ||
            playerInput.currentControlScheme == "Keyboard&Mouse") { return; }

        inputDirection = direction;

        Vector2 deltaValue = inputDirection * cursorSpeed;
        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;
        

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        //Debug.Log(deltaValue);

        
        /*
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
            (canvas, newPosition, mainCamera, out anchoredPosition);
        cursor.anchoredPosition = anchoredPosition;
        */
    }


    void LateUpdate()
    {
        if(playerInput.currentControlScheme == gamepadScheme && virtualMouse.added)
        {
            if (virtualMouse == null || !virtualMouse.added || Gamepad.current == null ||
            playerInput.currentControlScheme == "Keyboard&Mouse") { return; }

            inputDirection = Gamepad.current.leftStick.ReadValue();

            Vector2 deltaValue = inputDirection * cursorSpeed * Time.deltaTime;
            Vector2 currentPosition = virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + deltaValue;

            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, deltaValue);

            //Debug.Log(deltaValue);

            Vector2 anchoredPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (canvas, newPosition, mainCamera, out anchoredPosition);
            cursor.anchoredPosition = anchoredPosition;
        }
        else if (playerInput.currentControlScheme == mouseScheme && currentMouse != null)
        {
            Vector2 mousePosition = currentMouse.position.ReadValue();

            mousePosition.x = Mathf.Clamp(mousePosition.x, padding, Screen.width - padding);
            mousePosition.y = Mathf.Clamp(mousePosition.y, padding, Screen.height - padding);

            AnchorCursor(mousePosition);
        }



    }

    private void OnDrawGizmos()
    {

    }
}
