using UnityEngine;

public class LightTransition : MonoBehaviour
{
    [SerializeField] Light[] dayLights = new Light[1];
    [SerializeField] Light[] nightLights = new Light[1];

    [SerializeField] float[] lightIntensitys;

    private void Start()
    {
        foreach (Light light in dayLights)
        {
            //light.intensity = 30;
        }

        lightIntensitys[0] = dayLights[1].intensity;
    }

    public void StartTransition(bool dayToNight)
    {
        dayLights[0].intensity = 0;
    }
}
