using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class Dialogue : ScriptableObject
{
    public bool canMoveOnDialogueEnd;
    public bool canProceedOnDialogueEnd;
    public bool canRecycle;

    [TextArea(3, 5)]
    public string[] messages;
    public int[] spriteType;
    public Vector2[] delay;
    public bool[] isAuto;
    public float[] autoDelay;
    public bool[] isInvisible;
    public int[] emotion;   //아직
    public int[] talkerId;
    public int[] fontSize;
    public Vector2[] events;
    public int startEvent;
}
