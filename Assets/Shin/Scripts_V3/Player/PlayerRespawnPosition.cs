using UnityEngine;

[CreateAssetMenu(fileName = "PlayerRespawnPosition_", menuName = "Scriptable Objects/PlayerRespawnPosition")]
public class PlayerRespawnPosition : ScriptableObject
{
    public Vector3 defaultPosition;
    public Vector3[] position;
    public bool[] isRight = { true }; 
}
