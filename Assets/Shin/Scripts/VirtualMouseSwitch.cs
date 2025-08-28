using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseSwitch : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;

    VirtualMouseInput virtualMouseInput;

    string previousControlScheme = "";
    const string gamepadScheme = "Gamepad";
    const string mouseScheme = "Keyboard&Mouse";

    void Start()
    {
        virtualMouseInput = GetComponent<VirtualMouseInput>();
    }

    public void OnControlsChanged()
    {
        Debug.Log("Changed");
        if(playerInput.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {

        }
        else if (playerInput.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        {

        }
    }
}
