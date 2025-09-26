using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RHE00_Sample : MonoBehaviour, IEventInvoker
{
    [SerializeField] Dialogue dialogue;

    Image bubbleImage;
    Image eventImage;
    TMP_Text text;

    void Awake()
    {
        bubbleImage = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Dialogue");
        eventImage = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Event");
        text = GetComponentsInChildren<TMP_Text>().FirstOrDefault(x => x.name == "Text");
        CanStartEvent(false);
    }

    public void CanStartEvent(bool canStart)
    {
        eventImage.gameObject.SetActive(canStart);
    }

    public void ResetEvent()
    {
        DialogueManager.Instance.ResetDialogue(eventImage, true);
    }

    public void StartEvent()
    {
        DialogueManager.Instance.Dialogue(dialogue, eventImage, bubbleImage, text);
    }
}
