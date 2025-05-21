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

        Debug.Log(name + "   " + isS + isA);
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
        Vector2 adjustment = GameManager.Instance.book.adjustmentY[closeIndex];
        mesh.transform.DOLocalMoveY(isA ? 0 : (isS ? -y : y), adjustment.y)
            .SetDelay(adjustment.x).SetEase(Ease.InOutQuart);
    }

    void Update()
    {
        if(GameManager.Instance.book.isFlipping && isCurrent)
        {
            for (int i = 0; i < blendCount; i++)
            {
                float morphY = GameManager.Instance.book.morphs[closeIndex].position.y;
                mesh.SetBlendShapeWeight(i, (isActivate ? 1 - morphY : morphY) * 100);
            }
        }
    }
}
