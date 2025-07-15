using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BookObject : MonoBehaviour
{
    [Range(1, 10)] public int stage;

    [Space(10f)]
    public bool isRight;

    [Space(10f)]
    public Transform closeBone;
    public int closeIndex;
    public float height;

    [Space(10f)]
    public bool isStatic;
    public bool isActivate;
    public bool isCurrent;

    public virtual void Start()
    {
        //Find Close Bone
        SetBone();

        //Set Start Position With Book Animation
        StartCoroutine(SetStart());
    }

    void SetBone()
    {
        float dis = 100;

        Transform[] t = isRight ? GameManager.Instance.book.rightBones : GameManager.Instance.book.leftBones;
        for (int i  = 0; i < 10; i++)
        {
            float close = Vector3.Distance(transform.position, t[i].position);
            if (close < dis)
            {
                dis = close;
                closeBone = t[i];
                closeIndex = i;
            }
        }
    }

    IEnumerator SetStart()
    {
        transform.SetParent(closeBone);
        yield return new WaitForSeconds(0.6f);
        ResetParent();
    }

    //Set Parent as Default
    public void ResetParent()
    {
        transform.SetParent(GameManager.Instance.book.objectParents[stage]);
    }

    public virtual void SetObject()
    {

        Debug.Log(gameObject.name);
        // 1. Set isActivate, isStatic
        isActivate = GameManager.Instance.book.currentPage == stage;
        isStatic = (isActivate && isRight) || (!isActivate && !isRight);

        if(!isStatic)
        {
            transform.SetParent(GameManager.Instance.book.currentBones[closeIndex].transform);
        }

    }

    public virtual void AfterFlip()
    {

    }

    
}
