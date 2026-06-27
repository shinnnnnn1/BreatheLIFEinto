using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class HGE18_TurnWall : MonoBehaviour
{
    public bool _CanActivate;
    [SerializeField] BookController_V3 book;
    [SerializeField] HGE03_MatFadeNew fade;
    [SerializeField] CinemachineCamera fadedCam;
    [SerializeField] CinemachineCamera defaultCam;
    [SerializeField] Transform distortion;
    [SerializeField] Vector3[] distortionPos;
    [SerializeField] GameObject coll;
    [SerializeField] UnityEvent ev1, ev2;

    public void _SetCanActivate(bool act) => _CanActivate = act;
    public void _CheckActivate()
    {
        if(_CanActivate)
        {
            if (book.bookDir == 1)
            {
                fade._StartFade(false);
                fadedCam.Priority = -1;
                distortion.DOLocalMove(distortionPos[0], 1).SetEase(Ease.Linear);
                coll.SetActive(true);
                ev1.Invoke();
            }
            else if (book.bookDir == 2)
            {
                fade._StartFade(true);
                fadedCam.Priority = 10;
                distortion.DOLocalMove(distortionPos[1], 1).SetEase(Ease.Linear);
                coll.SetActive(false);
                ev2.Invoke();
            }
        }
    }
}
