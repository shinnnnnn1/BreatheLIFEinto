using UnityEngine;

public class RHE12_StopEmitting : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;

    public void _StopEmitting()
    {
        particle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
