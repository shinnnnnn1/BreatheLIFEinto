using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] RectTransform cursorTransform;
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform canvasRectTransform;
    [SerializeField] float cursorSpeed = 1000.0f;
    [SerializeField] float padding = 30.0f;

    [SerializeField] InputActionReference confirmAction;

    bool previousMouseState;
    Mouse virtualMouse;
    Mouse currentMouse;
    Camera mainCamera;

    string previousControlScheme = "";
    const string gamepadScheme = "Gamepad";
    const string mouseScheme = "Keyboard&Mouse";

    RaycastHit hit;
    ICursorInteractable cursorInteractable;

    void OnEnable()
    {
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        if (virtualMouse == null)
        {
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added)
        {
            InputSystem.AddDevice("VirtualMouse");
        }

        InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

        if (cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            Debug.Log(virtualMouse.position);
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        playerInput.onControlsChanged += OnControlsChanged;
    }

    void OnDisable()
    {
        if (virtualMouse != null && virtualMouse.added)
        {
            InputSystem.RemoveDevice(virtualMouse);
        }
        InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInput.onControlsChanged -= OnControlsChanged;
    }

    void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null) { return; }

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue() * cursorSpeed * Time.deltaTime;
        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        bool ButtonIsPressed = Gamepad.current.buttonEast.IsPressed();
        if (previousMouseState != ButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, ButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = ButtonIsPressed;
            //Debug.Log("Gamepad Button");
        }

        AnchorCursor(newPosition);
    }

    void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    void OnControlsChanged(PlayerInput input)
    {
        if (playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = false;

            if (currentMouse != null)
            {
                currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
                //InputState.Change(currentMouse.position, virtualMouse.position.ReadValue());
            }

            previousControlScheme = mouseScheme;
            Invoke("CursorActive", Time.deltaTime);
        }
        else if (playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        {
            Cursor.visible = false;

            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());

            previousControlScheme = gamepadScheme;
        }
    }

    void CursorActive() { cursorTransform.gameObject.SetActive(true); }

    void Update()
    {
        ChaseCursor();
        UpdateRay();
    }

    void ChaseCursor()
    {
        if (playerInput.currentControlScheme == mouseScheme && currentMouse != null)
        {
            Vector2 mousePosition = currentMouse.position.ReadValue();

            mousePosition.x = Mathf.Clamp(mousePosition.x, padding, Screen.width - padding);
            mousePosition.y = Mathf.Clamp(mousePosition.y, padding, Screen.height - padding);

            AnchorCursor(mousePosition);
        }
    }

    void UpdateRay()
    {
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = (cursorTransform.position - origin).normalized;

        if (Physics.Raycast(origin, direction, out hit, 100f))
        {
            ICursorInteractable target = hit.collider.GetComponent<ICursorInteractable>();

            if (cursorInteractable != target)
            {
                cursorInteractable?.OnExit();
            }
            else if (target != null)
            {
                target?.OnEnter();
            }

            cursorInteractable = target;
        }
        Debug.DrawRay(origin, direction * (hit.collider == null ? 100 : hit.distance));
    }

    public void OnClick()
    {
        cursorInteractable?.OnActivate();
    }
    public void OnRelease()
    {
        //Debug.Log("Off");
    }

    bool Ray()
    {
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = (cursorTransform.position - origin).normalized;
        RaycastHit hit;

        return Physics.Raycast(origin, direction, out hit, 100f);
    }

    public void CanHold(ICursorInteractable target)
    {
        cursorInteractable = target;
    }

    public void OnSwitch()
    {
        GameManager.Instance.Switch(false);
    }
}
