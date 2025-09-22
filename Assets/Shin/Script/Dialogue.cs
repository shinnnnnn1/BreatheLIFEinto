using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [TextArea(3, 5)]
    public string[] title;

    public bool[] isAuto;

    public Vector3[] isEvent;
}
