using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class RebindScript : MonoBehaviour
{
    [SerializeField] Image config;
    [SerializeField] InputActionAsset mainInput;
    [SerializeField] InputActionAsset defaultInput;
    [SerializeField] Button[] buttons;
    [SerializeField] Text[] texts;
    [SerializeField] GameObject selected;

    public void _StartConfig()
    {

        config.gameObject.SetActive(!config.gameObject.activeSelf);
        if (config.gameObject.activeSelf)
        {
            selected = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(selected);
        }

        texts[0].text = "Jump / Action = " + mainInput.FindAction("Jump").bindings[1].path
            + " / " + mainInput.FindAction("Action").bindings[1].path;
        texts[1].text = "SubmitUI = " + mainInput.FindAction("Click").bindings[1].path;
        texts[2].text = "DefaultUI = " + defaultInput.FindAction("Submit").bindings[1].path;
    }

    public void _RebindMainJumpAction()
    {
        Debug.Log("------------------------------------------------------------------------------");
        InputAction act = mainInput.FindAction("Jump");
        InputAction act2 = mainInput.FindAction("Action");

        var binding = act.bindings[1];
        if(binding.overridePath == "<Gamepad>/buttonEast")
        {
            act.ApplyBindingOverride(1, "<Gamepad>/buttonSouth");
            act2.ApplyBindingOverride(1, "<Gamepad>/buttonEast");
        }
        else
        {
            act.ApplyBindingOverride(1, "<Gamepad>/buttonEast");
            act2.ApplyBindingOverride(1, "<Gamepad>/buttonSouth");
        }
        binding = act.bindings[1];
        texts[0].text = "Jump / Action = " + mainInput.FindAction("Jump").bindings[1].overridePath
            + " / " + mainInput.FindAction("Action").bindings[1].overridePath;
        Debug.Log("originalPath: " + binding.path);
        Debug.Log("overridePath: " + binding.overridePath);
        Debug.Log("effectivePath: " + binding.effectivePath);
    }
    public void _RebindMainUISubmit()
    {
        Debug.Log("------------------------------------------------------------------------------");
        InputAction act = mainInput.FindAction("Click");
        var binding = act.bindings[1];
        if (binding.overridePath == "<Gamepad>/buttonEast")
        {
            act.ApplyBindingOverride(1, "<Gamepad>/buttonSouth");
        }
        else
        {
            act.ApplyBindingOverride(1, "<Gamepad>/buttonEast");
        }
        binding = act.bindings[1];
        texts[1].text = "SubmitUI = " + mainInput.FindAction("Click").bindings[1].overridePath;
        Debug.Log("originalPath: " + binding.path);
        Debug.Log("overridePath: " + binding.overridePath);
        Debug.Log("effectivePath: " + binding.effectivePath);
    }
    public void _RebindDefault(bool isEast)
    {
        Debug.Log("------------------------------------------------------------------------------");
        InputAction act = defaultInput.FindAction("Submit");

        var binding = act.bindings[1];
        if (binding.overridePath == "<Gamepad>/buttonEast")
        {
            act.ApplyBindingOverride(1, "<Gamepad>/buttonSouth");
        }
        else
        {
            act.ApplyBindingOverride(1, "<Gamepad>/buttonEast");
        }
        binding = act.bindings[1];
        texts[2].text = "DefaultUI = " + defaultInput.FindAction("Submit").bindings[1].overridePath;
        Debug.Log("originalPath: " + binding.path);
        Debug.Log("overridePath: " + binding.overridePath);
        Debug.Log("effectivePath: " + binding.effectivePath);
        
    }
}
