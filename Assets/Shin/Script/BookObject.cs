using System.Collections;
using UnityEngine;

public class BookObject : MonoBehaviour
{
    [Header("BookObject")]
    [Range(1, 10)] public int stage;
    [Space(20f)]
    public Transform model;
    public Transform closeBone;
    public int closeIndex;
    public bool isRight;
    public bool isStatic;
    public bool isActivate;
    public bool isCurrent;

    public virtual void Start()
    {
        //Debug.Log("Book");
        SetBone();
        StartCoroutine(SetStart());
    }

    void SetBone()
    {
        isRight = transform.position.x > 0;
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
        DeleteParent();
    }

    public void DeleteParent()
    {
        transform.SetParent(GameManager.Instance.book.objectParents[stage]);
    }

}
