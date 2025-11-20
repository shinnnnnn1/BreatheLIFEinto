using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RHE10_LoopEvent : MonoBehaviour
{
    [SerializeField] UnityEvent[] loop;
    [SerializeField] float[] loopTime;
    [SerializeField] bool forceStop;
    bool canLoop = true;

    public void _StartLoop()
    {
        StartCoroutine(LoopEvent());
    }

    IEnumerator LoopEvent()
    {
        while(canLoop)
        {
            for(int i = 0; i < loop.Length; i++)
            {
                loop[i].Invoke();
                yield return new WaitForSeconds(loopTime[i]);
            }
        }
    }

    public void _StopLoop()
    {
        canLoop = false;
        if(forceStop)
        {
            StopAllCoroutines();
        }
    }
}
