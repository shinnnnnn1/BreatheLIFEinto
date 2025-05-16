using UnityEngine;

public class BookObject : MonoBehaviour
{
    [Header("BookObject")]
    [Range(1, 10)] public int stage;
    [Space(20f)]
    public Transform model;
    public Transform closeBone;
    public Vector2 offset;

    public int closeIndex;
    public bool isRight;
    public bool isStatic;
    public bool isActivate;

    public virtual void Start()
    {
        //Debug.Log("Book");
        isRight = transform.position.x > 0;
        SetBone();
        SetParent();
    }

    void SetBone()
    {
        float dis = 100;
        foreach (Transform t in isRight ? GameManager.Instance.book.rightBones : GameManager.Instance.book.leftBones)
        {
            float close = Vector3.Distance(transform.position, t.position);
            if (close < dis)
            {
                dis = close;
                closeBone = t;
                
            }
        }

        for(int i  = 0; i < 10; i++)
        {
            
        }
    }

    public void SetParent()
    {
        transform.SetParent(closeBone);
        Invoke("NoParent", 0.1f);
    }

    void NoParent()
    {
        transform.SetParent(GameManager.Instance.book.objectParent);
    }
}
