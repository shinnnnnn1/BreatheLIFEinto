using System.Collections;
using UnityEngine;
using DG.Tweening;

public class StaticObject : BookObject
{
    SkinnedMeshRenderer mesh;
    int blendCount;
    [SerializeField] float y;

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
        y = transform.position.y;

        mesh.transform.localPosition = (transform.position.x > 0 ? Vector3.down : Vector3.up) * transform.position.y;

        Mesh mes = mesh.sharedMesh;
        blendCount = mes.blendShapeCount;
        for (int i = 0; i < blendCount; i++)
        {
            mesh.SetBlendShapeWeight(i, 100);
        }
    }


    public void SetObjext(bool isS, bool isA, bool isC)
    {
        isCurrent = isC;
        if(!isC) { return; }

        //Debug.Log(name + "   " + isS + isA);
        isStatic = isS;
        isActivate = isA;
        if (!isStatic)
        {
            transform.SetParent(GameManager.Instance.book.currentBones[closeIndex].transform);
        }

        MoveY(isS, isA);
        Invoke("DeleteParent", 3f);
    }

    void MoveY(bool isS, bool isA)
    {
        float value = isA ? 0 : (isS ? -y : y);
        float time = isA ? GameManager.Instance.book.curves[1].Evaluate(closeIndex) : 
            GameManager.Instance.book.curves[3].Evaluate(closeIndex);
        float delay = isA ? GameManager.Instance.book.curves[5].Evaluate(closeIndex) : 
            GameManager.Instance.book.curves[7].Evaluate(closeIndex);

        mesh.transform.DOLocalMoveY(value, time).SetDelay(delay)
            .SetEase(isA ? Ease.OutExpo : Ease.InExpo);

    }

    void Update()
    {
        if(GameManager.Instance.book.isFlipping && isCurrent)
        {
            for (int i = 0; i < blendCount; i++)
            {
                float morphY = isActivate ? GameManager.Instance.book.morphs[closeIndex].position.y :
                    GameManager.Instance.book.morphs[closeIndex + 10].position.y;
                mesh.SetBlendShapeWeight(i, (isActivate ? 1 - morphY : morphY) * 100);
            }
        }
    }
}
