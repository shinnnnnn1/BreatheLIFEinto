using DG.Tweening;
using UnityEngine;

public class PullableNew : MonoBehaviour, IInteractable
{
    [SerializeField] bool shouldFaceRight;
    [SerializeField] Vector2 direction;
    [SerializeField] Vector3 position;

    [Space(10f)]
    [SerializeField][Range(0, 5)] float pullValue;


    void Start()
    {
        
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
