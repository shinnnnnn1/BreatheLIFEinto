using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    private static FadeManager instance;
    public static FadeManager Instance => instance;

    void Awake()
    {
        Debug.Log(gameObject.name);
        instance = this;
    }

    [SerializeField] Image fadeImage;
    [SerializeField] float InDuration, outDuration;
    [SerializeField] Ease inEase, outEase;

    public void FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(0, InDuration).SetEase(inEase);
    }
    public void FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, outDuration).SetEase(outEase);
    }

    private void OnEnable()
    {
        FadeIn();
    }
}
