using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class Title : MonoBehaviour
{
    [SerializeField] Image fadeImage;
    bool isChanged = false;

    public void NextScene(int stage)
    {
        if(isChanged) { return; }
        fadeImage.DOFade(1, 1).SetEase(Ease.OutQuart);
        StartCoroutine(Next(stage));
        isChanged = true;
    }

    IEnumerator Next(int stage)
    {
        yield return new WaitForSeconds(1.5f);
        string stageName = "Stage_" + stage;
        Debug.Log(stageName);
    }
}
