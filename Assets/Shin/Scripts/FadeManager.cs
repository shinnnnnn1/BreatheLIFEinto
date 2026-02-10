using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    int s = 0;
    public void _RestartScene(int scene)
    {
        outDuration = 1;
        FadeOut();
        s = scene;
        Invoke("SceneChange", 2);
    }
    public void _RestartGame()
    {
        outDuration = 1;
        FadeOut();
        s = 0;
        GameManager.Instance._RestartGame();
        Invoke("SceneChange", 2);
    }

    void SceneChange()
    {
        SceneManager.LoadScene(s);
    }
}
