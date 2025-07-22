using UnityEngine;
using DG.Tweening;
using System.Collections;

public class NPC : BookObject
{
    MeshRenderer mesh;

    [Space(30f)]
    [SerializeField] Transform bottom;
    [SerializeField] Transform plane;

    public override void Start()
    {
        bottom.localEulerAngles = new Vector3(90, 0, 0);
        plane.gameObject.SetActive(false);
        base.Start();
    }

    public override void ResetParent()
    {
        transform.SetParent(GameManager.Instance.book.NPCParent);
    }

    public override void SetObject()
    {
        base.SetObject();
        StartCoroutine(FlipAnim());
    }

    IEnumerator FlipAnim()
    {
        if (isActivate)
        {
            //plane.gameObject.SetActive(true);
           // bottom.DOLocalRotate(new Vector3(0, 0, 0), 1.8f).SetDelay(0.5f);
        }
        else
        {
            bottom.DOLocalRotate(new Vector3(120, 0, 0), 2.0f).SetDelay(0.5f).SetEase(Ease.InOutQuad)
                .OnComplete(() => plane.gameObject.SetActive(false));
        }
        yield return new WaitForSeconds(1.25f);
        plane.gameObject.SetActive(isActivate);
        if (isActivate)
        {
            //plane.gameObject.SetActive(true);
           // bottom.DOLocalRotate(new Vector3(0, 0, 0), 1.5f).SetDelay(0f).SetEase(Ease,);
        }

        yield return new WaitForSeconds(1f);
       // plane.gameObject.SetActive(isActivate);

    }

    public override void AfterFlip()
    {

    }
}
