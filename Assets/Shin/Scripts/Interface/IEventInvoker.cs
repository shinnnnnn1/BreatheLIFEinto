using UnityEngine;

public interface IEventInvoker
{
    public void CanStartEvent(bool canStart);
    public void ResetEvent();
    public void StartEvent();
}
