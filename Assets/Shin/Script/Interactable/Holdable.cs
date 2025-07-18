using UnityEngine;

public class Holdable : MonoBehaviour, IInteractable
{
    Rigidbody rigid;
    Collider coll;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
    }

    public void OnEnter()
    {

    }
    public void OnExit()
    {

    }

    public void OnActivate()
    {
        rigid.mass = 1f;
        coll.sharedMaterial = GameManager.Instance.hMat[0];
    }
    public void OnDeactivate()
    {
        rigid.mass = 100f;
        coll.sharedMaterial = GameManager.Instance.hMat[1];
    }
}
