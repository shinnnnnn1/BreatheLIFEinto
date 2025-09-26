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

    

    
    public float autoDelay = 0.5f;

    public int current = -1;
    public bool canSkip = false;
    public bool canProceed = true;

    public void ResetDialogue(Image eventImage, bool isStart)
    {
        //会話の変数を初期化
        current = -1;
        canSkip = false;
        canProceed = true;

        //プレイヤーの操作を止め、会話に関する操作だけ可能な状態にする
        EventManager.Instance.playerController.SetCanMove(!isStart, false);
        EventManager.Instance.playerController.SetIsDialogue(isStart);

        //UIを表示/非表示
        eventImage.gameObject.SetActive(!isStart);
    }

    public void Dialogue(Dialogue dialogue, Image eventImage, Image bubbleImage, TMP_Text text)
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
            text.text = dialogue.messages[current];
        }
        //次の会話へ
        else if(canProceed)
        {
            StartCoroutine(DialogueCoroutine(dialogue, eventImage, bubbleImage, text));
        }
    }

    IEnumerator DialogueCoroutine(Dialogue d, Image eventImage, Image bubble, TMP_Text text)
    {
        canProceed = false;
        if (current >= 0)
        {
            //吹き出しが小さくなるアニメーション
            bubble.rectTransform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuint);
            yield return new WaitForSeconds(0.5f);

            //後Delay
            yield return new WaitForSeconds(d.delay[current].y);
        }
        
        current++;
        text.text = "";
        if(current < d.messages.Length)
        {
            if (d.events[current] > 0) { EventManager.Instance.PlayCutScene(d.events[current]); }

            //前Delay
            yield return new WaitForSeconds(d.delay[current].x);

            //吹き出しの表示/非表示
            bubble.enabled = !d.isInvisible[current];

            //吹き出しが大きくなるアニメーション
            bubble.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuint);
            yield return new WaitForSeconds(0.5f);

            //文字を表示
            canSkip = true;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < d.messages[current].Length; i++)
            {
                sb.Append(d.messages[current][i]);
                yield return new WaitForSeconds(0.1f);
                text.text = sb.ToString();
            }
            canSkip = false;
            canProceed = true;

            //自動化
            if (d.isAuto[current])
            {
                yield return new WaitForSeconds(autoDelay);
                StartCoroutine(DialogueCoroutine(d, eventImage, bubble, text));
            }
        }
        else
        {
            //会話の終了
            ResetDialogue(eventImage, false);
        }
    }
}
