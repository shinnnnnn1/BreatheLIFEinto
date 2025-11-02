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
}
