using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [Space(10f)]
    [SerializeField] float defaultAutoDelay = 1f;

    [Space(10f)]
    [SerializeField] int current = -1;
    [SerializeField] bool canSkip = false;
    [SerializeField] bool canProceed = true;

    [Space(10f)]
    [SerializeField] Sprite[] bubbleType;

    [Space(10f)]
    [SerializeField] Image[] bubbles;
    [SerializeField] TMP_Text[] texts;

    [SerializeField] DialoguePlayer currentDialoguePlayer;
    [SerializeField] Image currentBubble;
    [SerializeField] TMP_Text currentText;

    Image currentEventImage;

    public void ResetDialogue(DialoguePlayer player, Image eventImage, Image[] bubbleImages, TMP_Text[] dialogueTexts)
    {
        currentDialoguePlayer = player;

        bubbles = bubbleImages;
        texts = dialogueTexts;
        currentEventImage = eventImage;

        //UIを表示/非表示
        eventImage.gameObject?.SetActive(false);

        currentBubble = bubbleImages[0];
        currentText = dialogueTexts[0];

        currentText.text = "";
    }

    public void Dialogue(Dialogue dialogue)
    {
        //自動の場合
        if(current >= 0)
        {
            if (dialogue.isAuto[current]) { return; }
        }

        //会話アニメーションをスキップ
        if(canSkip)
        {
            canSkip = false;
            canProceed = true;
            StopAllCoroutines();
            currentText.text = dialogue.messages[current];
        }
        //次の会話へ
        else if(canProceed)
        {
            StartCoroutine(DialogueCoroutine(dialogue));
        }
    }

    IEnumerator DialogueCoroutine(Dialogue d)
    {
        canProceed = false;
        if (current >= 0)
        {
            //吹き出しが小さくなるアニメーション
            currentBubble.rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuint);
            yield return new WaitForSeconds(0.5f);

            //後Delay
            yield return new WaitForSeconds(d.delay[current].y);

            //イベントがある場合、再生
            if (d.events[current].y > 0) { EventManager.Instance.PlayCutScene((int)d.events[current].y); }
        }
        else
        {
            if (d.startEvent > 0) { EventManager.Instance.PlayCutScene(d.startEvent); }
        }

        current++;
        if(current < d.messages.Length)
        {
            //前Delay
            yield return new WaitForSeconds(d.delay[current].x);

            //イベントがある場合、再生
            if (d.events[current].x > 0) { EventManager.Instance.PlayCutScene((int)d.events[current].x); }

            //吹き出しの表示/非表示
            currentBubble.enabled = !d.isInvisible[current];
            currentText.enabled = !d.isInvisible[current];

            //吹き出しを話すキャラクターの吹き出しに変更
            currentBubble = bubbles[d.talkerId[current]];
            currentText = texts[d.talkerId[current]];

            //吹き出しのスプライトを変更
            currentBubble.sprite = bubbleType[d.spriteType[current]];

            //文字を初期化
            currentText.text = "";

            //フォントのサイズを変更
            currentText.fontSize = d.fontSize[current] > 0 ? d.fontSize[current] : 1;

            //吹き出しが大きくなるアニメーション
            currentBubble.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuint);
            yield return new WaitForSeconds(0.5f);

            //文字を表示
            canSkip = true;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < d.messages[current].Length; i++)
            {
                sb.Append(d.messages[current][i]);
                yield return new WaitForSeconds(0.1f);
                currentText.text = sb.ToString();
            }
            canSkip = false;
            canProceed = true;

            //自動化
            if (d.isAuto[current])
            {
                float autoD = d.autoDelay[current] == 0 ? defaultAutoDelay : d.autoDelay[current];
                yield return new WaitForSeconds(autoD);
                StartCoroutine(DialogueCoroutine(d));
            }
        }
        else
        {
            //会話の終了
            EndDiglogue(d);
        }
    }

    void EndDiglogue(Dialogue d)
    {
        //会話の変数を初期化
        current = -1;
        canSkip = false;
        canProceed = true;

        if (d.canRecycle)
        {
            currentDialoguePlayer.ChangeToRecycle();
        }
        else
        {
            currentDialoguePlayer.GetComponent<NPCObject>()?.SetDisableDialogue();
        }

        EventManager.Instance.playerController.SetIsDialogue(false);

        if (d.canMoveOnDialogueEnd)
        {
            currentEventImage?.gameObject.SetActive(true);
            EventManager.Instance.playerController.SetCanMove(true);
        }
        if (d.canProceedOnDialogueEnd)
        {
            EventManager.Instance.flipController.SetCanProceed(true);
        }
        
    }
}
