using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FlipでShapeの調整をするタイプ
/// </summary>
public class ShapeObject : BaseObject_V3
{
    [SerializeField] List<SkinnedMeshRenderer> mesh = new List<SkinnedMeshRenderer>();

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
                s.transform.localPosition = (isRight ? Vector3.down : Vector3.up) * height * 2;
            }
        }

        //全てのBlendShapeを100(潰れた状態)に設定
        //SetBlendShapes(100);
    }

    /// <summary>
    /// 
    /// </summary>
    /// 
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

    public override void SetHeight(float value, float time, float delay)
    {
        //Baseで条件に合ってないとreturnするようになってるからまた条件を考える必要はない
        base.SetHeight(value, time, delay);
        
        foreach (SkinnedMeshRenderer s in mesh)
        {
            //s.transform.DOLocalMoveY(value, time).SetDelay(delay).SetEase(isActivate ? Ease.OutQuint : Ease.InQuint);
        }

    }

    public void Update()
    {
        if (canShape)
        {
            float value = (1 - shape.position.y) * 100;
            SetBlendShapes(value);
        }
    }
}
