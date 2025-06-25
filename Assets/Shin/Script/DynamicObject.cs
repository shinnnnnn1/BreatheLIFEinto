using UnityEngine;

public class DynamicObject : StaticObject
{
    public Vector3 originPos;
    public Quaternion originQuat;

    public override void Start()
    {
        base.Start();
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
    }

}
