using UnityEngine;

public class Holdable : MonoBehaviour, IInteractable
{
    Rigidbody rigid;
    Collider coll;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        coll = GetComponentInChildren<Collider>();
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
    public void OnActivate(PlayerController p, out bool isPullable)
    {
        isPullable = false;
    }
    public void OnDeactivate()
    {
        rigid.mass = 100f;
        coll.sharedMaterial = GameManager.Instance.hMat[1];
    }
}
