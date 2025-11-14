using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueEvent_V3 : MonoBehaviour, IEventInvoker
{
    public Dialogue_V3 dialogue;
    public Dialogue_V3 recycled;

    [Space(10f)]
    public Image eventImage;
    public Image[] bubbles = new Image[1];
    public TMP_Text[] texts = new TMP_Text[1];

    void Awake()
    {
        eventImage = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Event");

        bubbles[0] = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Dialogue");
        texts[0] = GetComponentsInChildren<TMP_Text>().FirstOrDefault(x => x.name == "Text");

        OnEventEnter(false);
    }

    public void OnEventEnter(bool isEnter)
    {
        eventImage?.gameObject.SetActive(isEnter);
    }
    public void OnEventInvoke()
    {
        //DialogueManager_V3.Instance.NextDialogue();
    }

    public void SetNewDialogue(Dialogue_V3 newDialogue) => dialogue = newDialogue;
    public void SetNewRecycled(Dialogue_V3 newRecycled) => recycled = newRecycled;
}
