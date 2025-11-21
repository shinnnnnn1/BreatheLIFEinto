using UnityEngine;

public interface IFlipController
{
    public void SetCanProceed(bool can);
    public void CheckIsBookHorizontal(int bookAngle);
}
