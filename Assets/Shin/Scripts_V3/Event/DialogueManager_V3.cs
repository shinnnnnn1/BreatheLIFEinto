using DG.Tweening;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    [Tooltip("OnDialogueEnd, canMoveOnDialogueEnd, canProceedOnDialogueEnd")]
    [SerializeField] UnityEvent[] unityEvents;

    [SerializeField] EmotionObjectPool emotionObjectPool;

    private void Start()
    {
        emotionObjectPool = FindAnyObjectByType<EmotionObjectPool>();
    }

    void StartDialogue(DialogueEvent_V3 dialogue)
    {
        currentEvent = dialogue;

        bubbles = dialogue.bubbles;
        texts = dialogue.texts;

        currentBubble = bubbles[0];
        currentText = texts[0];

        currentText.text = "";
    }

    public void NextDialogue(DialogueEvent_V3 dialogue, Dialogue_V3 d)
    {
        //
        if(current < 0 && currentEvent == null) { StartDialogue(dialogue); }

        //自動の場合、ボタンを押しても何も起こらない
        if (current >= 0 && d.isAuto_AutoDelay_fontSize[current].x > 0) { return; }

        //会話アニメーションをスキップ
        if (canSkip)
        {
            canSkip = false;
            canProceed = true;
            StopAllCoroutines();
            currentText.text = d.messages[current];
        }
        //次の会話へ
        else if (canProceed)
        {
            StartCoroutine(DialogueCoroutine(d));
        }
    }
    IEnumerator DialogueCoroutine(Dialogue_V3 d)
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
            if (d.events[current].y > 0) { EventManager_V3.Instance.InvokeEvent((int)d.events[current].y); }
        }
        else if(d.startEvent > 0)
        {
            EventManager_V3.Instance.InvokeEvent(d.startEvent);
        }

            current++;
        if (current < d.messages.Length)
        {
            //前Delay
            yield return new WaitForSeconds(d.delay[current].x);

            //感情表現のイベント実行 (前Delayの前にするか後にするか悩んでる)
            if (d.emotion_Bubble[current].x > 0)
            {
                emotionObjectPool.InvokeEmotion((int)d.emotion_Bubble[current].x);
            }

            //イベントがある場合、再生
            if (d.events[current].x > 0) { EventManager_V3.Instance.InvokeEvent((int)d.events[current].x); }

            //吹き出しを話すキャラクターの吹き出しに変更
            currentBubble = bubbles[(int)d.talkerID_IsInvisible_isDialogueMotion[current].x];
            currentText = texts[(int)d.talkerID_IsInvisible_isDialogueMotion[current].x];

            //吹き出しの表示/非表示（Yが0だったら表示）
            currentBubble.enabled = d.talkerID_IsInvisible_isDialogueMotion[current].y == 0;
            currentText.enabled = d.talkerID_IsInvisible_isDialogueMotion[current].y == 0;

            //吹き出しのスプライトを変更
            currentBubble.sprite = bubbleType[(int)d.emotion_Bubble[current].y];

            //文字を初期化
            currentText.text = "";

            //フォントのサイズを変更
            currentText.fontSize = d.isAuto_AutoDelay_fontSize[current].z > 0 ? d.isAuto_AutoDelay_fontSize[current].z : 10;

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
            if (d.isAuto_AutoDelay_fontSize[current].x > 0)
            {
                float autoD = d.isAuto_AutoDelay_fontSize[current].y == 0 ? defaultAutoDelay : d.isAuto_AutoDelay_fontSize[current].y;
                yield return new WaitForSeconds(autoD);
                StartCoroutine(DialogueCoroutine(d));
            }
        }
        else
        {
            //会話の終了
            EndDialogue(d);
        }
    }

    void EndDialogue(Dialogue_V3 d)
    {
        //会話の変数を初期化
        current = -1;
        canSkip = false;
        canProceed = true;

        //会話が終わった後
        currentEvent.EndEvent();
        unityEvents[0].Invoke();

        //会話を変更するか
        if (d.canRecycle) { currentEvent.SwitchToRecycle(); }

        //キャラクターを動ける状態にするか
        if (d.canMoveOnDialogueEnd) { unityEvents[1].Invoke(); }

        //次のページに進める状態にするか
        if (d.canProceedOnDialogueEnd) { unityEvents[2].Invoke(); }

        currentEvent = null;
    }
}
