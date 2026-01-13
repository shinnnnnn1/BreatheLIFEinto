using UnityEngine;

[CreateAssetMenu(fileName = "DialogueV3", menuName = "Scriptable Objects/DialogueV3")]
public class Dialogue_V3 : ScriptableObject
{
    public bool canMoveOnDialogueEnd;
    public bool canProceedOnDialogueEnd;
    public bool canRecycle;

    [TextArea(1, 1)]
    public string[] messages;

    public Vector3[] emotion_Bubble_MiddleEmotion;
    public Vector2[] delay;
    public Vector3[] isAuto_AutoDelay_FontSize;
    public Vector3[] talkerID_IsInvisible_IsDialogueMotion;
    public Vector3[] matPreset_ButtonInvisible;

    public Vector3[] events;
    public int startEvent;
    public bool isLoop;
}
