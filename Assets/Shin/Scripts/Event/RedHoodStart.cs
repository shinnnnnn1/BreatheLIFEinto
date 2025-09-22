using UnityEngine;

public class RedHoodStart : EventManager
{
    public void MovingRedHood()
    {
        startRedHood.isKinematic = false;
        startRedHood.linearVelocity = new Vector3(2, 0, 0);
    }
    public void StopRedHood()
    {
        startRedHood.linearVelocity = Vector3.zero;
    }

}
