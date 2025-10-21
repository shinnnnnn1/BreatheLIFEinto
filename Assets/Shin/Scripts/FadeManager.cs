using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeManager : MonoBehaviour
{
    private static FadeManager instance;
    public static FadeManager Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration;
    [SerializeField] Ease ease;

    public void FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(0, fadeDuration).SetEase(ease);
    }
    public void FadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, fadeDuration).SetEase(ease);
    }

    private void Start()
    {
        FadeIn();
    }
}
