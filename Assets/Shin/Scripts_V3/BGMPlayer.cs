using System.Collections;
using UnityEngine;
using static System.TimeZoneInfo;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] AudioClip currentClip;
    [SerializeField] AudioSource currentSource;
    [SerializeField] float transition = 1;

    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioSource[] sources;

    private void Start()
    {
        ChangeBGM(0);
    }

    public void ChangeBGM(int num)
    {
        //現在のBGMのFadeOut
        if(currentSource != null)
        {
            FadeOut(currentSource, 0);
        }

        //新しいBGMのFadeIn
        AudioSource newSource = currentSource == sources[0] ? sources[1] : sources[0];
        newSource.clip = clips[num];
        newSource.Play();
        FadeIn(newSource, 1);
    }

    IEnumerator FadeIn(AudioSource source, float goal)
    {
        for (float i = 0; i < goal; i += transition * Time.deltaTime)
        {
            source.volume = i;
            yield return null;
        }
    }
    IEnumerator FadeOut(AudioSource source, float goal)
    {
        for (float i = 1; i > goal; i -= transition * Time.deltaTime)
        {
            source.volume = i;
            yield return null;
        }
        source.Stop();
    }
}
