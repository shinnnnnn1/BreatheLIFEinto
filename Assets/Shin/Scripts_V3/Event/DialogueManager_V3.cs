using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager_V3 : MonoBehaviour
{
    #region SINGLETON
    private static DialogueManager_V3 instance;
    public static DialogueManager_V3 Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    [Space(10f)]
    [SerializeField] Sprite[] bubbleType;
    [SerializeField] float defaultAutoDelay = 1f;

    [Space(10f)]
    [SerializeField] int current = -1;
    [SerializeField] bool canSkip = false;
    [SerializeField] bool canProceed = true;

    [Space(10f)]
    [SerializeField] DialogueEvent_V3 currentEvent;

    [SerializeField] Image[] bubbles;
    [SerializeField] TMP_Text[] texts;

    [SerializeField] Image currentBubble;
    [SerializeField] TMP_Text currentText;

    void StartDialogue(DialogueEvent_V3 dialogue)
    {

    }

    public void NextDialogue(DialogueEvent_V3 dialogue)
    {
        //会話の初期設定
        if(current < 0 && currentEvent == null) { StartDialogue(dialogue); }


        //会話アニメーションをスキップ
        if (canSkip)
        {
            canSkip = false;
            canProceed = true;
            StopAllCoroutines();
            //currentText.text = dialogue.messages[current];
        }
        //次の会話へ
        else if (canProceed)
        {
            //StartCoroutine(DialogueCoroutine(dialogue));
        }
    }
    IEnumerator DialogueCoroutine(Dialogue_V3 d)
    {
        yield return null;
    }

    void EndDialogue()
    {

    }
}
