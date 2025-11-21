using UnityEngine;
using DG.Tweening;

public class Distortion : MonoBehaviour
{

    [SerializeField] bool isFlip;

    public void OnActivateFlip(Vector3 value, float time, Ease ease)
    {
        if(isFlip)
        {
            transform.DOScale(value, time).SetEase(ease);
        }
    }

    public void OnActivate(Vector3 value, float time, Ease ease)
    {
        transform.DOScale(value, time).SetEase(ease);
    }
}
