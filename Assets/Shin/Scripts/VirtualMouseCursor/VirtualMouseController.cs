using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


public class VirtualMouseController : MonoBehaviour
{
    [Space(10f)]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerModel_V3 playerModel;
    [SerializeField] VirtualMouseModel model;
    [SerializeField] CinemachineCamera cursorCam;
    [SerializeField] GraphicRaycaster[] raycasters;

    VirtualMouseView view;
    VirtualMouseInput virtualMouseInput;
    RectTransform cursor;
    Camera mainCamera;

    string previousControlScheme = "";
    const string gamepadScheme = "Gamepad";
    const string mouseScheme = "Keyboard&Mouse";

    const string playerActionMap = "Player";
    const string UIActionMap = "UI";

    [SerializeField] bool canPress = false;
    [SerializeField] bool isPressing = false;

    [SerializeField] Collider trackingColl, pressingColl, releasingColl;

    ICursorInteractable cursorInteractable;

    bool setStart = false;

    void Start()
    {
        view = GetComponent<VirtualMouseView>();
        virtualMouseInput = GetComponentInChildren<VirtualMouseInput>();
        cursor = virtualMouseInput.cursorGraphic.GetComponent<RectTransform>();
        mainCamera = Camera.main;

        if(playerInput.currentActionMap.name == playerActionMap)
        {
            SetCursorMode(false);
        }
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        if (!virtualMouseInput.enabled) { return; }

        if (playerInput.currentControlScheme == mouseScheme && Mouse.current != null)
        {
            CursorChase();
        }
        else if(playerInput.currentControlScheme == gamepadScheme && Gamepad.current != null)
        {
            CursorPadding();
        }

        UpdateRay();
    }

    public void SetCursorMode(bool activate)
    {
        if (!setStart) { setStart = true; }
        else if (!playerModel.canMove) { return; }

        Vector2 mousePos = Mouse.current.position.ReadValue();

        string nextActionMap = activate ? UIActionMap : playerActionMap;
        playerInput.SwitchCurrentActionMap(nextActionMap);

        if (playerInput.currentControlScheme == mouseScheme && activate)
        {
            view.CursorChase(mousePos);
        }

        model.isCursorMode = activate;
        virtualMouseInput.enabled = activate;
        view.SetCursorVisible(activate);
        foreach (GraphicRaycaster r in raycasters)
        {
            r.enabled = activate;
        }

        Debug.Log("Current Map is " + playerInput.currentActionMap.name);
    }

    public void OnControlsChanged()
    {
        Debug.Log("Changed to " + playerInput.currentControlScheme);
        if (playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            Mouse.current.WarpCursorPosition(virtualMouseInput.virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        }
        else if (playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        {
            previousControlScheme = gamepadScheme;
        }
    }

    void CursorChase()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.x = Mathf.Clamp(mousePosition.x, model.cursorPadding, Screen.width - model.cursorPadding);
        mousePosition.y = Mathf.Clamp(mousePosition.y, model.cursorPadding, Screen.height - model.cursorPadding);
        view.CursorChase(mousePosition);
    }

    void CursorPadding()
    {
        Vector2 virtualMousePos = virtualMouseInput.virtualMouse.position.ReadValue();
        virtualMousePos.x = Mathf.Clamp(virtualMousePos.x, model.cursorPadding, Screen.width - model.cursorPadding);
        virtualMousePos.y = Mathf.Clamp(virtualMousePos.y, model.cursorPadding, Screen.height - model.cursorPadding);
        view.CursorPadding(virtualMousePos);
    }

    void UpdateRay()
    {
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = (cursor.position - origin).normalized;

        view.UpdateRay(origin, direction, model.interactingDistance, IsHit());

        if (isPressing) { return; }

        view.ChangeCursorImage(IsHit() ? 2 : 0);

        if (trackingColl != null)
       {
            if (cursorInteractable == null)
            {
                cursorInteractable = trackingColl.GetComponent<ICursorInteractable>();
                cursorInteractable?.OnEnter();
            }
       }
       else if(cursorInteractable != null)
       {
            cursorInteractable?.OnExit();
            cursorInteractable = null;
       }
    }

    public void OnPressed()
    {
        isPressing = true;
        view.ChangeCursorImage(1);

        if (trackingColl != null && trackingColl != pressingColl)
        {
            pressingColl = trackingColl;
            cursorInteractable.OnPressed();
        }
    }
    public void OnReleased()
    {
        isPressing = false;
        if(trackingColl != null && trackingColl == pressingColl)
        {
            cursorInteractable.OnReleased();
        }
    }

    bool IsHit()
    {
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = (cursor.position - origin).normalized;
        if (Physics.Raycast(origin, direction, out RaycastHit raycastHit, model.interactingDistance, model.interactableLayerMask))
        {
            trackingColl = raycastHit.collider;
            return true;
        }
        else
        {
            ResetTracking();
            return false;
        }
    }

    public void ResetTracking()
    {
        trackingColl = null;
        pressingColl = null;
        releasingColl = null;
    }
}
