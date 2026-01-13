using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueEvent_V3 : MonoBehaviour, IEventInvoker
{
    [SerializeField] Dialogue_V3 dialogue;
    [SerializeField] Dialogue_V3 recycled;

    [Space(10f)]
    public Image eventImage;
    public Image[] bubbles = new Image[1];
    public TMP_Text[] texts = new TMP_Text[1];
    public Image[] buttons = new Image[1];
    public RectTransform customRect;

    [SerializeField] bool isEventPlaying = false;
    [SerializeField] MeshCollider npcCylinder;

    void Awake()
    {
        eventImage = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Event");

        bubbles[0] = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Dialogue");
        texts[0] = GetComponentsInChildren<TMP_Text>().FirstOrDefault(x => x.name == "Text");
        buttons[0] = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Button");

        npcCylinder = GetComponentsInChildren<MeshCollider>().FirstOrDefault();

        OnEventEnter(false);
    }

    public void OnEventEnter(bool isEnter)
    {
        eventImage?.gameObject.SetActive(isEnter);
    }
    public void OnEventInvoke()
    {
        //会話を開始する場合
        if (!isEventPlaying)
        {
            //EventImageを非表示。最初の一回だけ
            OnEventEnter(false);
            isEventPlaying = true;
        }
        DialogueManager_V3.Instance.NextDialogue(this, dialogue);
    }

    public void EndEvent()
    {
        isEventPlaying = false;

        //繰り返し会話ができる場合
        if (dialogue.canRecycle)
        {
            //EventImageを表示
            OnEventEnter(true);
        }
        //一回限りの会話の場合はコライダーを無効化する
        else
        {
            if(npcCylinder != null)
                npcCylinder.enabled = false;
        }
    }

    public void SwitchToRecycle() => dialogue = recycled;
    public void SetNewDialogue(Dialogue_V3 newDialogue) => dialogue = newDialogue;
    public void SetNewRecycled(Dialogue_V3 newRecycled) => recycled = newRecycled;
}
