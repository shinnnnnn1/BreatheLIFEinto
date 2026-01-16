using UnityEngine;

public interface IInteractable
{
    public void OnEnter(bool isRight);
    public void OnExit();
    public void OnPullStart();
    public void OnPullEnd();
}
