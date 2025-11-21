using Unity.Cinemachine;
using UnityEngine;

public class RHE19_CameraShake : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] CinemachineCamera cam;
    [SerializeField] float currentTime;
    [SerializeField] float duration;
    [SerializeField] bool isPlaying;

    public void _Play()
    {
        currentTime = 0;
        isPlaying = true;
    }

    public void Update()
    {
        if(isPlaying)
        {
            currentTime += Time.deltaTime;
            cam.Lens.FieldOfView = curve.Evaluate(currentTime);
            if(currentTime >= duration)
            {
                isPlaying = false;
            }
        }
    }
}
