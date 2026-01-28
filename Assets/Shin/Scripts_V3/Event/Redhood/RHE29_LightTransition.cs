using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.GlobalVariables;
using UnityEngine.Rendering.HighDefinition;

public class RHE29_LightTransition : MonoBehaviour
{
    HDAdditionalLightData l;

    [SerializeField] float duration;
    [SerializeField] Ease ease;
    float defaultValue;

    private void Start()
    {
        l = GetComponent<HDAdditionalLightData>();
        defaultValue = l.lightDimmer;
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
