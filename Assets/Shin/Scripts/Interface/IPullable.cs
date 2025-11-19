using UnityEngine;

public interface IPullable
{
    public void OnActivate(PlayerController_V3 p, bool isRight);
    public void OnDeactivate();
}
