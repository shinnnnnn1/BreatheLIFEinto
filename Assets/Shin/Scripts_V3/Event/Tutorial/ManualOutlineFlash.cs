using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ManualOutlineFlash : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] bool isTransparent;
    [SerializeField] bool canFlash = true;
    bool isLooping = false;
    Tween t;

    void Start()
    {
        if(isTransparent)
        {
            mat.SetFloat("_ValueMultiply", 0);
        }
        mat.SetFloat("_Value", 0);
    }
    private void OnDestroy()
    {
        mat.SetFloat("_Value", 1);
        mat.SetFloat("_ValueMultiply", isTransparent ? 0 : 1);
    }

    public void _StartFlash()
    {
        if (!canFlash) { return; }
        isLooping = true;
        mat.SetFloat("_Value", 0);
        StopAllCoroutines();
        StartCoroutine(LoopEvent());
    }
    IEnumerator LoopEvent()
    {
        while (isLooping)
        {
            t = DOVirtual.Float(0, 1, 0.5f,
            onVirtualUpdate: (tweenValue) => { mat.SetFloat("_Value", tweenValue); }).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);

            t = DOVirtual.Float(1, 0, 0.5f,
            onVirtualUpdate: (tweenValue) => { mat.SetFloat("_Value", tweenValue); }).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void _StartFlash2()
    {
        if (!canFlash) { return; }
        isLooping = true;
        mat.SetFloat("_Value", 1);
        StopAllCoroutines();
        StartCoroutine(LoopEvent2());
    }
    IEnumerator LoopEvent2()
    {
        while (isLooping)
        {
            t = DOVirtual.Float(1, 0, 0.5f,
            onVirtualUpdate: (tweenValue) => { mat.SetFloat("_Value", tweenValue); }).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);

            t = DOVirtual.Float(0, 1, 0.5f,
            onVirtualUpdate: (tweenValue) => { mat.SetFloat("_Value", tweenValue); }).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void _StopFlash(bool forceStop)
    {
        isLooping = false;
        if (forceStop) { StopAllCoroutines(); t.Kill(); }
        mat.SetFloat("_Value", 0);
    }

    public void _SetTransparent(bool isTransparent) => mat.SetFloat("_ValueMultiply", isTransparent ? 0 : 1);
    public void _SetCanFlash() => canFlash = false;
}
