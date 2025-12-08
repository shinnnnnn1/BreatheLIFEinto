using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TitleBookButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] UnityEvent onSelect, onDeselect;

    public void OnSelect(BaseEventData eventData)
    {
        onSelect.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onDeselect.Invoke();
    }
}
