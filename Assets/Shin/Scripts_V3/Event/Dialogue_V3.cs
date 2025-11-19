using UnityEngine;

[CreateAssetMenu(fileName = "DialogueV3", menuName = "Scriptable Objects/DialogueV3")]
public class Dialogue_V3 : ScriptableObject
{
    public bool canMoveOnDialogueEnd;
    public bool canProceedOnDialogueEnd;
    public bool canRecycle;

    [TextArea(1, 1)]
    public string[] messages;

    public Vector2[] emotion_Bubble;
    public Vector2[] delay;
    public Vector3[] isAuto_AutoDelay_fontSize;
    public Vector3[] talkerID_IsInvisible_isDialogueMotion;

    public Vector2[] events;
    public int startEvent;
}
