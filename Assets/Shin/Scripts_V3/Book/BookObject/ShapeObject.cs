using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FlipでShapeの調整をするタイプ
/// </summary>
public class ShapeObject : BaseObject_V3
{
    [SerializeField] List<SkinnedMeshRenderer> mesh = new List<SkinnedMeshRenderer>();

    Transform[] shapeAct, shapeDeact;
    Transform currentShape;
    bool canShape;

    Rigidbody rigid;

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
        SetBlendShapes(100);

        //Holdableの場合、Rigidbodyが必要
        rigid = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 本からボーンをもらう。ShapeObjectだけShapeをもらう。初期設定専用
    /// </summary>
    /// <seealso cref="BookController_V3.Start()"/>
    public override void GetBones(Transform[] pL, Transform[] pR, Transform[] pLC, Transform[] pRC, Transform[] sA, Transform[] sD)
    {
        base.GetBones(pL, pR, pLC, pRC, sA, sD);
        shapeAct = sA;
        shapeDeact = sD;
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



    /// <summary>
    /// Heightの調整。Delayとは関係なくページのFlipに合わせる。
    /// </summary>
    /// <seealso cref="BaseObject_V3.FlipHeight(BookModel_V3, bool)"/>
    public override void SetHeight(float value, float time, float delay, Ease ease)
    {
        //一番上がアーマチュアの場合もあるからBaseも実行
        base.SetHeight(value, time, delay, ease);

        //全てのSkinnedMeshRendererのHeightのHeightを調整
        foreach (SkinnedMeshRenderer s in mesh)
        {
            s.transform.DOLocalMoveY(value, time).SetDelay(delay).SetEase(ease);
        }

        if (rigid != null)
        {
            rigid.isKinematic = true;
        }
    }

    /// <summary>
    /// オブジェクトのタイプごとにモーションを実行。
    /// </summary>
    /// <seealso cref="BookController_V3.Flip()"/>
    public override void FlipMotion(BookModel_V3 model, bool isAct)
    {
        base.FlipMotion(model, isAct);

        if (isCurrent && isAct == isActivate)
        {
            //ShapeObjectの場合、CurrentShapeを設定し、Shapeを実行
            currentShape = isActivate ? shapeAct[closeIndex] : shapeDeact[closeIndex];
            canShape = true;
        }
    }

    void Update()
    {
        //Shapeが可能なら
        if (canShape)
        {
            //Shape値を計算する
            float value = (1 - currentShape.position.y) * 100;

            //数値を入れてShapeを調整
            SetBlendShapes(value);
        }
    }



    public override void AfterFlip(Transform[] objectParents)
    {
        //条件はBaseで確認
        base.AfterFlip(objectParents);

        //Shapeを停止
        canShape = false;

        if(rigid != null && isActivate)
        {
            rigid.isKinematic = false;
        }

    }
}
