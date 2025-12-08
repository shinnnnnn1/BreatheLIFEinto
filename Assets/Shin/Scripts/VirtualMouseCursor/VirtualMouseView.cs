using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;

public class VirtualMouseView : MonoBehaviour
{
    [SerializeField] VirtualMouseInput virtualMouseInput;
    [SerializeField] RectTransform cursor;
    [SerializeField] Image cursorImage;

    public Sprite[] cursorImages;

    void Awake()
    {
        Cursor.visible = false;
        virtualMouseInput = GetComponentInChildren<VirtualMouseInput>();
        cursor = virtualMouseInput.cursorGraphic.GetComponent<RectTransform>();
        cursorImage = cursor.GetComponent<Image>();
    }

    public void CursorPadding(Vector2 position)
    {
        InputState.Change(virtualMouseInput.virtualMouse.position, position);
    }

    public void CursorChase(Vector2 position)
    {
        cursor.anchoredPosition = position;
    }

    public void SetCursorVisible(bool visible)
    {
        cursor.gameObject.SetActive(visible);
    }

    public void ResetCursorPosition(float width, float height)
    {

    }

    public void ChangeCursorImage(int type)
    {
        cursorImage.sprite = cursorImages[type];
    }
    
    public void UpdateRay(Vector3 origin, Vector3 direction, float distance, bool isHit)
    {
        Debug.DrawRay(origin, direction * distance, isHit ? Color.green : Color.red);
    }
}
