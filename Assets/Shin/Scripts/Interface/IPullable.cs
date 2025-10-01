using UnityEngine;

public interface IPullable
{
    public void OnActivate(PlayerController p, bool isRight);
    public void OnDeactivate();
}
