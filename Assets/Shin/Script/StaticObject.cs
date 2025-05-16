using UnityEngine;

public class StaticObject : BookObject
{
    SkinnedMeshRenderer mesh;
    int blendCount;

    public override void Start()
    {
        //Debug.Log("Static");
        SetMorph();
        base.Start();
    }

    void SetMorph()
    {
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        model = mesh.gameObject.transform;

        mesh.transform.localPosition = (transform.position.x > 0 ? Vector3.down : Vector3.up) * transform.position.y * 2;

        Mesh mes = mesh.sharedMesh;
        blendCount = mes.blendShapeCount;
        for (int i = 0; i < blendCount; i++)
        {
            mesh.SetBlendShapeWeight(i, 100);
        }
    }


    public void SetObjext(bool isS, bool isA)
    {

        isStatic = isS;
        isActivate = isA;
        Debug.Log(gameObject.name + isStatic + isActivate);


        if (!isStatic)
        {
            
        }
    }
}
