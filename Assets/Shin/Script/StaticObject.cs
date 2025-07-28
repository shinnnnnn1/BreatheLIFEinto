using DG.Tweening;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StaticObject : BookObject
{
    protected SkinnedMeshRenderer[] mesh;

    float y;

    Transform armature;

    public override void Start()
    {
        SetMorph();

        //BookObject.Start
        base.Start();
    }

    void SetMorph()
    {
        //Get Mesh Component
        mesh = GetComponentsInChildren<SkinnedMeshRenderer>();

        //Set Mesh Height and Blend Shapes and Disable
        height = transform.position.y;
        foreach(var m in mesh)
        {
            m.transform.localPosition = (transform.position.x > 0 ? Vector3.down : Vector3.up) * transform.position.y;
            m.SetBlendShapeWeight(0, 100);
            m.SetBlendShapeWeight(1, 100);
            m.enabled = false;
        }

        if(armature != null)
        {
            armature.localPosition = (transform.position.x > 0 ? Vector3.down : Vector3.up) * transform.position.y;
        }
    }

    public override void SetObject()
    {
        base.SetObject();
        HeightAdjustment();
        foreach (var m in mesh)
        {
            m.enabled = true;
        }
    }

    void HeightAdjustment()
    {
        float value = isActivate ? 0 : (isStatic ? -height : height);
        float time = isActivate ? GameManager.Instance.book.curvesValue[2].Evaluate(closeIndex) :
            GameManager.Instance.book.curvesValue[3].Evaluate(closeIndex);
        float delay = isActivate ? GameManager.Instance.book.curvesDelay[2].Evaluate(closeIndex) :
            GameManager.Instance.book.curvesDelay[3].Evaluate(closeIndex);
        foreach (var m in mesh)
        {
            m.transform.DOLocalMoveY(value, time).SetDelay(delay)
            .SetEase(isActivate ? Ease.InOutExpo : Ease.InExpo);
        }

        //If Armature move Armature
        if (armature != null)
        {
            armature?.transform.DOLocalMoveY(value, time).SetDelay(delay)
            .SetEase(isActivate ? Ease.InOutExpo : Ease.InExpo);
        }
    }

    public void Update()
    {
        if (GameManager.Instance.book.isFlipping && mesh[0].enabled)
        {
            float morphHeight = isActivate ? GameManager.Instance.book.morphs[closeIndex].position.y :
                    GameManager.Instance.book.morphs[closeIndex + 10].position.y;
            foreach (var m in mesh)
            {
                m.SetBlendShapeWeight(0, (isActivate ? 1 - morphHeight : morphHeight) * 100);
                m.SetBlendShapeWeight(1, (isActivate ? 1 - morphHeight : morphHeight) * 100);
            }
        }
    }

    public override void AfterFlip()
    {
        if(GameManager.Instance.book.currentPage != stage)
        {
            foreach (var m in mesh)
            {
                m.enabled = false;
            }
        }
    }
}
