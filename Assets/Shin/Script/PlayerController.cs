using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Rigidbody rigid;

    public Vector3 inputDirection;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        
    }

    public void Jump()
    {

    }
}
