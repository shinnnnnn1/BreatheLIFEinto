using UnityEngine;

public class DynamicObject : StaticObject
{
    public override void Start()
    {
        base.Start();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
    }

}
