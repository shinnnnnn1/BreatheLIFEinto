using DG.Tweening;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    Material mat;
    [SerializeField] float startValue;
    [SerializeField] float currentValue;

    private void Start()
    {
        mat = GetComponent<Image>().material;
        mat.SetFloat("_Alpha", startValue);
        currentValue = startValue;
    }
    private void OnDestroy()
    {
        mat.SetFloat("_Alpha", 1);
    }

    public void Fade(float value)
    {
        //StartCoroutine(FadeCoroutine(value));

        Ease fadeEase = mat.GetFloat("_Alpha") > value ? Ease.OutQuint : Ease.Linear;

        DOVirtual.Float(mat.GetFloat("_Alpha"), value, 1,
                onVirtualUpdate: (tweenValue) => { mat.SetFloat("_Alpha", tweenValue); })
                .SetEase(fadeEase);

    }

    IEnumerator FadeCoroutine(float goal)
    {
        //값이 내려가야 한다면
        if(currentValue > goal)
        {
            while (currentValue > goal)
            {
                currentValue -= Time.deltaTime;
                mat.SetFloat("_Alpha", currentValue);
                yield return null;
            }
        }
        //값이 올라야 한다면
        else
        {
            while (currentValue < goal)
            {
                currentValue += Time.deltaTime;
                mat.SetFloat("_Alpha", currentValue);
                yield return null;
            }
        }
    }
}
