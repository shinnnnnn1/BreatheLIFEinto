using DG.Tweening;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioSource[] sources;
    [SerializeField] float transition = 1;

    public void _PlaySE(int num)
    {
        sources[0].PlayOneShot(clips[num]);
    }
    public void _PlayLoopSE(int num)
    {
        sources[1].clip = clips[num];
        sources[1].Play();
    }
    public void _StopSE()
    {
        sources[0].Stop();
    }
    public void _StopLoopSE()
    {
        sources[1].Stop();
    }

    public void _SetVolume0(float vol)
    {
        sources[0].volume = vol;
    }
    public void _SetVolume1(float vol)
    {
        sources[1].volume = vol;
    }
    public void _SetVolume2(float vol)
    {
        sources[2].volume = vol;
    }

    public void _SetTransition(float t) => transition = t;
    public void _FadeLoopSE(float value)
    {
        sources[1].DOFade(value, transition).SetEase(Ease.Linear)
            .OnComplete(() => { if (value == 0) { _StopLoopSE(); _SetVolume1(1); } });
    }
}
