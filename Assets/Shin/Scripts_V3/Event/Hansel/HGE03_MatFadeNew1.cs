using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HGE03_MatFadeNew1 : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] string param = "_Alpha";
    [SerializeField] float startValue;
    [SerializeField] float goal;
    [SerializeField] float duration;
    [SerializeField] Ease inEase, outEase;

    [Space(10f)]
    [SerializeField] float currentValue;

    private void Start()
    {
        mat.SetFloat(param, startValue);
        currentValue = mat.GetFloat(param);
    }
    private void OnDestroy()
    {
        mat.SetFloat(param, startValue);
    }

    public void _StartFade(bool fadeIn)
    {
        currentValue = mat.GetFloat(param);
        float g = fadeIn ? goal : startValue;
        Ease ease = fadeIn ? inEase : outEase;

        DOVirtual.Float(currentValue, g, duration, 
            onVirtualUpdate : (tweenValue) => { mat.SetFloat(param, tweenValue); }).SetEase(ease);
    }
}
