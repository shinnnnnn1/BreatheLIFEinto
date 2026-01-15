using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// EmotionSpaceの中のEmotion_nに入れ、TypeやDelayなどを設定
/// Emotionの再生は
/// </summary>
public class EmotionInvoker : MonoBehaviour
{
    [Range(0, 20)] public int emotionType;

    [SerializeField] Vector3 scale = Vector3.one;
    [SerializeField] float delay;
    [SerializeField] bool isWorld;

    [SerializeField] Transform[] child;

    [Space(10f)]
    [SerializeField] bool isLoop;
    [SerializeField] bool loopDelay;

    public void InvokeEmotion(ParticleSystem emotion)
    {
        //親を設定し、位置を初期化
        emotion.transform.SetParent(transform);
        emotion.transform.localPosition = Vector3.zero;
        //emotion.transform.localScale = scale;

        child = transform.GetComponentsInChildren<Transform>().Where(w => w != this.transform).ToArray();
        foreach (Transform t in child)
        {
            t.localScale = scale;
        }

        //
        if(isWorld)
        {
            emotion.transform.SetParent(null);
        }

        //
        StartCoroutine(PlayEmotion(emotion));
    }

    IEnumerator PlayEmotion(ParticleSystem emotion)
    {
        yield return new WaitForSeconds(delay);
        emotion.Play();

        while(isLoop)
        {
            yield return new WaitForSeconds(delay);
            emotion.Stop();
            emotion.Play();
        }
    }

    public void SetLoop(bool loop) => isLoop = loop;
}
