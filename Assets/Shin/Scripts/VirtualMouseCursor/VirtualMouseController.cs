using System.Transactions;
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
    [SerializeField] MeshCollider cursorConfiner;
    [SerializeField] float confinerZ;
    [SerializeField] float currentZ;

    public Vector2 zoomDirection;

    VirtualMouseView view;
    [SerializeField] VirtualMouseInput virtualMouseInput;
    RectTransform cursor;
    Camera mainCamera;

    string previousControlScheme = "";
    const string gamepadScheme = "Gamepad";
    const string mouseScheme = "Keyboard&Mouse";

    const string playerActionMap = "Player";
    const string UIActionMap = "UI";

    #pragma warning disable CS0414
    [SerializeField] bool canPress = false;
    [SerializeField] bool isPressing = false;
    #pragma warning restore CS0414



    [SerializeField] Collider currentColl, trackingColl, pressingColl;

    ICursorInteractable cursorInteractable;
    RaycastHit hit;

    public Vector3 hitPoint;

    [SerializeField] bool setStart = false;
    [SerializeField] bool canChange = false;

    void Start()
    {
        view = GetComponent<VirtualMouseView>();
        virtualMouseInput = GetComponentInChildren<VirtualMouseInput>();
        cursor = virtualMouseInput.cursorGraphic.GetComponent<RectTransform>();
        mainCamera = Camera.main;
        currentZ = cursorConfiner.transform.localScale.z;

        if(playerInput.currentActionMap.name == playerActionMap)
        {
            SetCursorMode(false);
        }
    }

    void Update()
    {
        if(virtualMouseInput.enabled && zoomDirection.magnitude > 0.01f)
        {
            confinerZ += zoomDirection.x * Time.deltaTime;
            confinerZ = Mathf.Clamp(confinerZ, -0.1f, 0.1f);

            currentZ += confinerZ;
            currentZ = Mathf.Clamp(currentZ, 1, 7);
            cursorConfiner.transform.localScale = new Vector3(1, 1, currentZ);
        }
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

        //누른채로 커서가 너무 빨라서 튕긴 상황
        //누른 상황 + currentColl이 있는데 Tracking Pressing Coll 은 없는 상황?
        if (isPressing &&  currentColl != null&& trackingColl == null && pressingColl == null)
        {
            //나중에 이 조건 다시 설명해야될듯?
            OnCanceled();
        }

        UpdateRay();
    }

    public void SetCursorMode(bool activate)
    {
        if (!setStart) { setStart = true; }
        else if (canChange) { }
        else if (!playerModel.canMove) { return; }

        cursorCam.gameObject.SetActive(activate);

        Vector2 mousePos = Mouse.current.position.ReadValue();

        string nextActionMap = activate ? UIActionMap : playerActionMap;
        playerInput.SwitchCurrentActionMap(nextActionMap);

        if (playerInput.currentControlScheme == mouseScheme && activate)
        {
            view.CursorChase(mousePos);
        }

        model.isCursorMode = activate;
        //virtualMouseInput.enabled = true;

        virtualMouseInput.cursorSpeed = activate ? 1000 : 0;

        view.SetCursorVisible(activate);

        /*
        foreach (GraphicRaycaster r in raycasters)
        {
            r.enabled = activate;
        }
        */

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
        //Debug.Log(mousePosition);
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

        //레이를 막는. 타겟안되게 하는 콜라이더에 막히면 리턴하는 조건
        //if (IsIgnore()) { view.ChangeCursorImage(0); return; }

        //누르고있는 상태에선 리턴
        if (isPressing) { return; }

        view.ChangeCursorImage(IsHit() ? 2 : 0);

        //TrackingColl이 없다가 생겼을때 TrackingColl이 있을때
        if (trackingColl != null)
        {
            //CursorInteractable도 없다면
            if (cursorInteractable == null)
            {
                currentColl = trackingColl;
                cursorInteractable = trackingColl.GetComponent<ICursorInteractable>();
                cursorInteractable?.OnEnter();
            }
            //TrackingColl은 있는데 CurrentColl와 다를때
            else if (currentColl != trackingColl)
            {
                currentColl = trackingColl;
                cursorInteractable?.OnExit();
                cursorInteractable = trackingColl.GetComponent<ICursorInteractable>();
                cursorInteractable?.OnEnter();
            }
        }
        //TrackingColl은 없고 CursorInteractable만 남았을때
        else if (cursorInteractable != null)
        {
            cursorInteractable?.OnExit();
            cursorInteractable = null;
            currentColl = null;
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
            pressingColl = null;
            cursorInteractable.OnReleased();
        }
    }

    public void OnCanceled()
    {
        cursorInteractable?.OnCanceled();
        isPressing = false;
        currentColl = null;
        view.ChangeCursorImage(0);
    }



    bool IsHit()
    {
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = (cursor.position - origin).normalized;
        if(IsIgnore())
        {
            ResetTracking();
            return false;
        }
        if (Physics.Raycast(origin, direction, out hit, model.interactingDistance, model.interactableLayerMask) )
        {
            trackingColl = hit.collider;
            hitPoint = hit.point;
            return true;
        }
        else
        {
            ResetTracking();
            return false;
        }
    }

    bool IsIgnore()
    {
        Vector3 origin = mainCamera.transform.position;
        Vector3 direction = (cursor.position - origin).normalized;
        if (Physics.Raycast(origin, direction, out hit, model.interactingDistance, model.ignoreLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetTracking()
    {
        trackingColl = null;
        pressingColl = null;
    }

    public void SetCanChange(bool can)
    {
        canChange = can;
    }
}
