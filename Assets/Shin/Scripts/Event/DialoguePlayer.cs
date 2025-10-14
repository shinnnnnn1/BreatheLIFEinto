using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialoguePlayer : MonoBehaviour
{
    public Dialogue dialogue;
    public Dialogue recycleDialogue;

    [SerializeField] Image[] bubbleImages = new Image[1];
    [SerializeField] TMP_Text[] texts = new TMP_Text[1];

    [SerializeField] Image eventImage;

    void Awake()
    {
        eventImage = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Event");

        bubbleImages[0] = GetComponentsInChildren<Image>().FirstOrDefault(x => x.name == "Dialogue");
        texts[0] = GetComponentsInChildren<TMP_Text>().FirstOrDefault(x => x.name == "Text");
        CanStartEvent(false);
    }

    public void CanStartEvent(bool canStart)
    {
        eventImage?.gameObject.SetActive(canStart);
    }

    public void ResetEvent()
    {
        DialogueManager.Instance.ResetDialogue(this, eventImage, bubbleImages, texts);
    }

    public void PlayEvent()
    {
        DialogueManager.Instance.Dialogue(dialogue);
    }

    public void PlayAutoEvent()
    {
        EventManager.Instance.playerController.SetDialogueAuto(this);
        DialogueManager.Instance.ResetDialogue(this, eventImage, bubbleImages, texts);
        DialogueManager.Instance.Dialogue(dialogue);
    }

    public void ChangeToRecycle()
    {
        dialogue = recycleDialogue;
    }

    public void SetNewDialogue(Dialogue newDialogue)
    {
        dialogue = newDialogue;
    }

    public void SetNewRecycleDialogue(Dialogue newRecycleDialogue)
    {
        recycleDialogue = newRecycleDialogue;
    }
}
