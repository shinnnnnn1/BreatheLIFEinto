using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMousePadding : MonoBehaviour
{
    VirtualMouseInput virtualMouseInput;

    [SerializeField] float padding = 30f;

    void Awake()
    {
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    void LateUpdate()
    {
        Vector2 virtualMousePos = virtualMouseInput.virtualMouse.position.ReadValue();
        virtualMousePos.x = Mathf.Clamp(virtualMousePos.x, padding, Screen.width - padding);
        virtualMousePos.y = Mathf.Clamp(virtualMousePos.y, padding, Screen.height - padding);
        InputState.Change(virtualMouseInput.virtualMouse.position, virtualMousePos);
    }
}
