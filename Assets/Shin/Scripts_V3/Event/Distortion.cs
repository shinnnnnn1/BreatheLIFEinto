using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Distortion : MonoBehaviour
{

    [SerializeField] bool isFlip;
    bool isLocked;

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

    public void LockObject(bool onLock, int bookDir)
    {
        isLocked = onLock;
    }
    private void Update()
    {
        if (isLocked)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
