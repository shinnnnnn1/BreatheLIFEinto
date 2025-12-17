using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class RHE14_RotSelect : MonoBehaviour
{
    [SerializeField] Transform trans, transModel;
    [SerializeField] Vector3[] rot = new Vector3[2];
    [SerializeField] float time;
    [SerializeField] Ease ease;

    public void _LocalRotate()
    {
        float y = trans.eulerAngles.y;
        Debug.Log(y);

        Vector3 r = y < 180 ? rot[0] : rot[1];

        trans.DORotate(r, time).SetEase(ease).OnComplete(() => transModel.localEulerAngles = trans.localEulerAngles);
    }
}
