using UnityEngine;

[CreateAssetMenu(fileName = "DialogueV2", menuName = "Scriptable Objects/DialogueV2")]
public class Dialogue_V2 : ScriptableObject
{
    public bool canMoveOnDialogueEnd;
    public bool canProceedOnDialogueEnd;
    public bool canRecycle;

    [TextArea(2, 1)]
    public string[] messages;

    public Vector2[] emotion_Bubble;
    public Vector2[] delay;
    public Vector3[] isAuto_AutoDelay_fontSize;
    public Vector3[] talkerID_IsShake_IsInvisible;

    //어차피 처음에 이벤트 딜레이 뒤에 이벤트가필요한거면
    //처음 이벤트에서 딜레이를 기다리고 이벤트 넣으면 되잖아
    public Vector2[] events;
}
