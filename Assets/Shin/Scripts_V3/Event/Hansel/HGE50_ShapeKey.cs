using DG.Tweening;
using System.Collections;
using UnityEngine;

public class HGE50_ShapeKey : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer smr;
    [SerializeField] float value;

    [SerializeField] int num;
    [SerializeField] float delay;

    [SerializeField] float[] loops;
    [SerializeField] float[] values;

    public void _StartShapeKey()
    {
        //StartCoroutine(ShapeKeyCoroutine());
    }
    IEnumerator ShapeKeyCoroutine()
    {
        for (int i = 0; i < loops.Length; i++)
        {
            DOVirtual.Float(0, 100, 0.5f,
            onVirtualUpdate: (tweenValue) =>
            {
                values[i] = tweenValue;
                smr.SetBlendShapeWeight(i, values[i]);
                Debug.Log("asdasdsad");
            })
            .SetLoops(2, LoopType.Yoyo);
            yield return loops[i];
        }
    }


    private void Update()
    {
        //smr.SetBlendShapeWeight(num, value);
    }


    private void Start()
    {
        _StartShapeKey();
        DOVirtual.Float(0, 100, 0.5f,
            onVirtualUpdate: (tweenValue) =>
            {
                value = tweenValue;
                smr.SetBlendShapeWeight(num, value);
            })
            .SetLoops(2, LoopType.Yoyo).SetDelay((float)num/2);
        /*
        DOVirtual.Float(0, 100, 0.5f,
            onVirtualUpdate: (tweenValue) => { smr.SetBlendShapeWeight(num, tweenValue); } )
            .SetLoops(2, LoopType.Yoyo);
        */
    }
}
