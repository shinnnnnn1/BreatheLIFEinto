using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeController : MonoBehaviour
{
    Image fadeImage;

    void Awake()
    {
        fadeImage = GetComponent<Image>();
    }

    public void ImageFade(float value, float time, Ease easing)
    {
        fadeImage.DOFade(value, time);
    }    
}
