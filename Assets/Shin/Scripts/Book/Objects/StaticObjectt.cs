using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StaticObjectt : BaseObject
{
    public List<SkinnedMeshRenderer> mesh = new List<SkinnedMeshRenderer>();

    Transform shape;
    bool canShape;

    public override void Start()
    {
        base.Start();

        //全てのSkinnedMeshRendererを参照
        foreach (Transform t in children)
        {
            SkinnedMeshRenderer s = t.GetComponent<SkinnedMeshRenderer>();
            if (s != null)
            {
                mesh.Add(s);
            }
        }

        //全てのBlendShapeを100(潰れた状態)に設定
        SetBlendShapes(100);
    }

    void SetBlendShapes(float value)
    {
        foreach (SkinnedMeshRenderer s in mesh)
        {
            Mesh m = s.sharedMesh;
            for (int i = 0; i < m.blendShapeCount; i++)
            {
                s.SetBlendShapeWeight(i, value);
            }
        }
    }



    public override void SetBookObject(int currentStage, Transform[] currentBones, Transform[] shapes, BookModel model)
    {
        base.SetBookObject(currentStage, currentBones, shapes, model);
        if(isCurrent)
        {
            int i = isActivate ? 0 : 9;
            shape = shapes[i + closeIndex];
            canShape = true;
        }
    }

    public override void AfterFlip(Transform[] objectParents)
    {
        base.AfterFlip(objectParents);
        canShape = false;
    }

    void Update()
    {
        if(canShape)
        {
            float value = (1 - shape.position.y) * 100;
            SetBlendShapes(value);
        }
    }

}
