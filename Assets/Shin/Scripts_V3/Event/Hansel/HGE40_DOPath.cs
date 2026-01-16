using UnityEngine;
using DG.Tweening;

public class HGE40_DOPath : MonoBehaviour
{
    [SerializeField] Transform trans;
    [SerializeField] Transform transss;
    [SerializeField] Vector3[] paths;
    [SerializeField] float duration;

    public void _StartPath()
    {
        trans.DOLocalPath(paths, duration, PathType.CatmullRom);
    }




    private void Start()
    {
        _StartPath();
    }
}
