using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TitleBookButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public int i;
    [SerializeField] bool canChange;
    [SerializeField] UnityEvent onSelect, onDeselect, onConfirm;
    
    public void OnSelect(BaseEventData eventData)
    {
        onSelect.Invoke();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if(canChange)
        {
            onDeselect.Invoke();
        }
    }
    public void OnConfirm()
    {
        onConfirm.Invoke();
    }

    public void _SetCanChange(bool change) => canChange = change;

    public void _ChangeSceneTrigger(int sceneNum)
    {
        GameManager.Instance.ChangeScene(sceneNum);
    }
}
