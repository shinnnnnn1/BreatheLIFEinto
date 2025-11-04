using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModel_V3", menuName = "Scriptable Objects/PlayerModel_V3")]
public class PlayerModel_V3 : ScriptableObject
{
    [Header("Player Info")]
    public bool isRight = true;
    public bool isTurning = false;
    public bool isHolding = false;

    [Space(10f)]
    public bool canControl = true;
    public bool canMove = false;
    public bool canChange = false;
    public bool canProceed = false;

    [Space(10f)]
    public bool canJump = true;
    public bool canSwitch = true;
    public bool canAnim = true;

    [Space(10f)]
    public float moveSpeed = 2.0f;

    [Header("Movement Settings")]
    public float accelerationTime = 0.1f;
    public float decelerationTime = 0.1f;
    public float defaultSpeed = 2.0f;
    public float holdingSpeed = 0.5f;

    [Space(10f)]
    public float jumpPow;

    [Space(10f)]
    public float turnTime = 0.1f;

    [Header("Colliders")]
    public Vector3 boxOffset = Vector3.zero;
    public Vector3 boxSize = Vector3.one;
    public Vector3 sphereOffset = Vector3.zero;
    public float sphereRadius = 0.1f;

    [Space(10f)]
    public Vector3 jumpBoxOffset = Vector3.zero;
    public Vector3 jumpBoxSize = Vector3.one;
    public float jumpBoxDistance = 0.5f;
    public LayerMask groundLayer;

    [Space(10f)]
    public Vector3 hitBoxOffset = Vector3.zero;
    public Vector3 hitBoxSize = Vector3.one;
    public float hitBoxDistance = 0.5f;
    public LayerMask hitLayer;
    public Vector2 jointAnchorRight = Vector3.zero;

    [Space(10f)]
    public float eventSphereRadius = 0.5f;
    public LayerMask eventLayer;
}
