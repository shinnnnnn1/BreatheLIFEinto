using UnityEngine;

public class DynamicObject : StaticObject
{
    Rigidbody rigid;
    float ta;

    public override void Start()
    {
        rigid = GetComponentInChildren<Rigidbody>();
        rigid.isKinematic = true;
        base.Start();
    }

    public override void SetObject()
    {
        //need to set isRight, CloseBone, Close Index...
        rigid.isKinematic = true;
        base.SetObject();
    }

    public override void AfterFlip()
    {
        base.AfterFlip();
        if(mesh.enabled)
        {
            rigid.isKinematic = false;
        }
    }
}
