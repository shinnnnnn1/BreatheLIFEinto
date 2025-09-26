using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea(3, 5)]
    public string[] messages;
    public Vector2[] delay;
    public bool[] isAuto;
    public bool[] isInvisible;
    public int[] events;
}
