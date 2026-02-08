using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class RHE29_LightTransition : MonoBehaviour
{
    [SerializeField] HDAdditionalLightData l;

    [SerializeField] float defaultValue;
    [SerializeField] float duration;
    [SerializeField] Ease ease;

    private void Start()
    {
        //l = GetComponent<HDAdditionalLightData>();
    }

    public void _Fade(float v)
    {
        float start = l.lightDimmer;
        float value = v == -1.0f ? defaultValue : v;

        DOVirtual.Float(start, value, duration,
            onVirtualUpdate: (tweenValue) => { l.lightDimmer = tweenValue; })
            .SetEase(ease);
    }
}
