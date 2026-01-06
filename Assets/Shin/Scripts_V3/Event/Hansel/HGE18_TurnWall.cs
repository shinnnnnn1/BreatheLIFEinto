using Unity.Cinemachine;
using UnityEngine;

public class HGE18_TurnWall : MonoBehaviour
{
    public bool _CanActivate;
    [SerializeField] BookController_V3 book;
    [SerializeField] HGE03_MatFadeNew fade;
    [SerializeField] CinemachineCamera fadedCam;

    public void _SetCanActivate(bool act) => _CanActivate = act;
    public void _CheckActivate()
    {
        if(_CanActivate)
        {
            if (book.bookDir == 1)
            {
                fade._StartFade(false);
                fadedCam.gameObject.SetActive(false);
            }
            else if (book.bookDir == 2)
            {
                fade._StartFade(true);
                fadedCam.gameObject.SetActive(true);
            }
        }
    }
}
