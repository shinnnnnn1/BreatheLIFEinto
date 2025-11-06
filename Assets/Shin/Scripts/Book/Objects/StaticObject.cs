using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : BaseObject
{
    public List<SkinnedMeshRenderer> mesh = new List<SkinnedMeshRenderer>();

    Transform shape;
    bool canShape;

    Rigidbody rigid;

    [SerializeField] Animator anim;
    [SerializeField] bool isAnim;
    [SerializeField] float animDelay;

    public override void Start()
    {
        rigid = GetComponent<Rigidbody>();

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
        if(rigid != null) { rigid.isKinematic = true; }

        base.SetBookObject(currentStage, currentBones, shapes, model);
        if(isCurrent)
        {
            //Updateでシェイプキーを調整できるようにする
            int i = isActivate ? 0 : 9;
            shape = shapes[i + closeIndex];
            canShape = true;

            if(isAnim)
            {
                Invoke("AnimTime", animDelay);
            }
        }
    }

    void AnimTime()
    {
        anim.SetTrigger(isActivate ? "Activate" : "Deactivate");
    }

    public override void AfterFlip(Transform[] objectParents)
    {
        base.AfterFlip(objectParents);
        canShape = false;

        if (rigid != null && isCurrent) { rigid.isKinematic = false; }
    }

    public override void SetHeight(float value, float time, float delay)
    {
        base.SetHeight(value, time, delay);

        foreach(SkinnedMeshRenderer s in mesh)
        {
            s.transform.DOLocalMoveY(value, time).SetDelay(delay)
            .SetEase(isRight ? Ease.OutQuint : Ease.OutQuint);
        }
    }

    public void Update()
    {
        if(canShape)
        {
            float value = (1 - shape.position.y) * 100;
            SetBlendShapes(value);
        }
    }
}
