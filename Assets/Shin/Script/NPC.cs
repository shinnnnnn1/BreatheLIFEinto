using UnityEngine;
using DG.Tweening;
using System.Collections;

public class NPC : BookObject
{
    [Space(30f)]
    [SerializeField] Transform bottom;
    MeshRenderer mesh;

    public override void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        mesh.gameObject.SetActive(false);

        bottom.localEulerAngles = new Vector3(90, 0, 0);

        base.Start();
    }

    public override void ResetParent()
    {
        transform.SetParent(GameManager.Instance.book.NPCParent);
    }

    public override void SetObject()
    {
        base.SetObject();

        float delay = 1f;
        if(isActivate && !isRight)
        {
            delay = 1.25f;
        }
        Invoke("FlipAnim", delay);
    }

    void FlipAnim()
    {
        mesh.gameObject.SetActive(true);
        if (isActivate)
        {
            bottom.DOLocalRotate(new Vector3(0, 0, 0), 1f).SetEase(Ease.InOutCubic);
        }
        else
        {
            float value = isRight ? 50f : 90f;
            float time = isRight ? 0.8f : 1f;
            bottom.DOLocalRotate(new Vector3(value, 0, 0), time).SetEase(Ease.OutQuad)
                    .OnComplete(() => mesh.gameObject.SetActive(false));
        }
    }

    public override void AfterFlip()
    {

    }
}
