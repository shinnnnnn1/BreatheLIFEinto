using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;

public class HGE18_TurnWall : MonoBehaviour
{
    public bool _CanActivate;
    [SerializeField] BookController_V3 book;
    [SerializeField] HGE03_MatFadeNew fade;
    [SerializeField] CinemachineCamera fadedCam;
    [SerializeField] Transform distortion;
    [SerializeField] Vector3[] distortionPos;
    [SerializeField] GameObject coll;

    public void _SetCanActivate(bool act) => _CanActivate = act;
    public void _CheckActivate()
    {
        if(_CanActivate)
        {
            if (book.bookDir == 1)
            {
                fade._StartFade(false);
                fadedCam.gameObject.SetActive(false);
                distortion.DOLocalMove(distortionPos[0], 1).SetEase(Ease.Linear);
                coll.SetActive(true);
            }
            else if (book.bookDir == 2)
            {
                fade._StartFade(true);
                fadedCam.gameObject.SetActive(true);
                distortion.DOLocalMove(distortionPos[1], 1).SetEase(Ease.Linear);
                coll.SetActive(false);
            }
        }
    }
}
