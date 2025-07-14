using UnityEngine;

public class Holdable : MonoBehaviour, IInteractable
{
    Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public void OnEnter()
    {

    }
    public void OnExit()
    {

    }
    public void OnActivate()
    {

    }
    public void OnDeactivate()
    {

    }
}
