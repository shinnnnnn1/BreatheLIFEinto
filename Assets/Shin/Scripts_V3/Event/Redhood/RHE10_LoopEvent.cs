using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RHE10_LoopEvent : MonoBehaviour
{
    [SerializeField] UnityEvent[] loop;
    [SerializeField] float[] loopTime;
    [SerializeField] bool forceStop;
    [SerializeField] bool canLoop = true;
    [SerializeField] bool isLooping;
    [SerializeField] UnityEvent onStop;

    public void _StartLoop()
    {
        if (isLooping) { return; }
        canLoop = true;
        StopAllCoroutines();
        StartCoroutine(LoopEvent());
    }

    IEnumerator LoopEvent()
    {
        isLooping = true;
        while (canLoop)
        {
            for(int i = 0; i < loop.Length; i++)
            {
                loop[i].Invoke();
                yield return new WaitForSeconds(loopTime[i]);
            }
        }
        isLooping = false;
    }

    public void _StopLoop()
    {
        canLoop = false;
        isLooping = false;
        if (forceStop)
        {
            StopAllCoroutines();
        }
        onStop.Invoke();
    }
}
