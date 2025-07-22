using System.Collections;
using UnityEngine;
using DG.Tweening;

public class StaticObject : BookObject
{
    protected SkinnedMeshRenderer mesh;

    int blendCount;
    [SerializeField] float y;

    public override void Start()
    {
        SetMorph();

        //BookObject.Start
        base.Start();
    }

    void SetMorph()
    {
        //Get Mesh Component
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        //Set Mesh Height
        height = transform.position.y;
        mesh.transform.localPosition = (transform.position.x > 0 ? Vector3.down : Vector3.up) * transform.position.y;

        //Set BlendShapes
        Mesh mes = mesh.sharedMesh;
        blendCount = mes.blendShapeCount;
        for (int i = 0; i < blendCount; i++)
        {
            mesh.SetBlendShapeWeight(i, 100);
        }

        //Hide Mesh
        mesh.enabled = false;
    }

    public override void SetObject()
    {
        base.SetObject();
        HeightAdjustment();
        mesh.enabled = true;
    }

    void HeightAdjustment()
    {
        float value = isActivate ? 0 : (isStatic ? -height : height);
        float time = isActivate ? GameManager.Instance.book.curvesValue[2].Evaluate(closeIndex) :
            GameManager.Instance.book.curvesValue[3].Evaluate(closeIndex);
        float delay = isActivate ? GameManager.Instance.book.curvesDelay[2].Evaluate(closeIndex) :
            GameManager.Instance.book.curvesDelay[3].Evaluate(closeIndex);

        mesh.transform.DOLocalMoveY(value, time).SetDelay(delay)
            .SetEase(isActivate ? Ease.InOutExpo : Ease.InExpo);
    }

    public void Update()
    {
        if (GameManager.Instance.book.isFlipping && mesh.enabled)
        {
            for (int i = 0; i < blendCount; i++)
            {
                float morphHeight = isActivate ? GameManager.Instance.book.morphs[closeIndex].position.y :
                    GameManager.Instance.book.morphs[closeIndex + 10].position.y;
                mesh.SetBlendShapeWeight(i, (isActivate ? 1 - morphHeight : morphHeight) * 100);
            }
        }
    }

    public override void AfterFlip()
    {
        if(GameManager.Instance.book.currentPage != stage)
        {
            mesh.enabled = false;
        }
    }
}
