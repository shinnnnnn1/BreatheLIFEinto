using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] AudioSource currentSource;
    [SerializeField] float transition = 1;

    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioSource[] sources;

    [SerializeField] Ease[] ease;

    private void Start()
    {
        ChangeBGM(0);
    }

    public void ChangeTransition(float t) => transition = t;

    public void ChangeBGM(int num)
    {
        //現在のBGMのFadeOut
        if(currentSource != null)
        {
            currentSource.DOFade(0, transition).SetEase(ease[0]);
        }

        //新しいBGMのFadeIn
        AudioSource newSource = currentSource == sources[0] ? sources[1] : sources[0];
        currentSource = newSource;
        currentSource.clip = clips[num];
        currentSource.Play();
        currentSource.DOFade(1, transition).SetEase(ease[1]);
    }


    /*
    public void ChangeBGM(int num)
    {
        //現在のBGMのFadeOut
        if (currentSource != null)
        {
            StartCoroutine(FadeOut(currentSource, 0));
        }

        //新しいBGMのFadeIn
        AudioSource newSource = currentSource == sources[0] ? sources[1] : sources[0];
        newSource.clip = clips[num];
        newSource.Play();
        StartCoroutine(FadeIn(newSource, 1));

        newSource.DOFade(1, 1);
    }

    IEnumerator FadeIn(AudioSource source, float goal)
    {
        source.volume = 0;
        for (float i = 0; i < goal; i += transition * Time.deltaTime)
        {
            source.volume = i;
            yield return null;
        }
        currentSource = source;
    }
    IEnumerator FadeOut(AudioSource source, float goal)
    {
        source.volume = 1;
        for (float i = 1; i > goal; i -= transition * Time.deltaTime * 1.5f)
        {
            source.volume = i;
            yield return null;
        }
        source.Stop();
    }
    */
}
