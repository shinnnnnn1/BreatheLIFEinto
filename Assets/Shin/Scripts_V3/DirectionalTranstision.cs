using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DirectionalTranstision : MonoBehaviour
{
    [SerializeField] Light l;
    [SerializeField] HDAdditionalLightData lightData;

    [SerializeField] float defaultInt;
    [SerializeField] float[] duration;
    [SerializeField] Ease[] ease;

    private void Start()
    {

        _StartFade(-1);
    }
    public void _StartFade(float v)
    {
        float startInt = l.intensity;
        float intValue = v == -1.0f ? defaultInt : v;

        DOVirtual.Float(startInt, intValue, duration[0],
            onVirtualUpdate: (tweenValue) => { l.intensity = tweenValue; })
            .SetEase(ease[0]);
    }
    public void _StartFade2(float v)
    {
        float startDim = lightData.shadowDimmer;
        float dimValue = v == -1.0f ? 1 : v;

        DOVirtual.Float(startDim, dimValue, duration[1],
            onVirtualUpdate: (tweenValuee) => { lightData.shadowDimmer = tweenValuee; })
            .SetEase(ease[1]);
    }

    public void _SetDimmer(float v)
    {
        lightData.shadowDimmer = v;
    }
}
