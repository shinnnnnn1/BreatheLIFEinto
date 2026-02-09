using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DirectionalTranstision : MonoBehaviour
{
    [SerializeField] Light l;
    [SerializeField] HDAdditionalLightData lightData;

    [SerializeField] float[] intensity, temperature, intT, temT;
    [SerializeField] Ease[] ease;

    public void _FadeIntensity(int i)
    {
        float startInt = l.intensity;

        DOVirtual.Float(startInt, intensity[i], intT[i],
            onVirtualUpdate: (tweenValue) => { l.intensity = tweenValue; })
            .SetEase(ease[i]);
    }
    public void _FadeTemperature(int i)
    {
        float startTem = l.colorTemperature;

        DOVirtual.Float(startTem, temperature[i], temT[i],
            onVirtualUpdate: (tweenValuee) => { l.colorTemperature = tweenValuee; })
            .SetEase(ease[i]);
    }
}
