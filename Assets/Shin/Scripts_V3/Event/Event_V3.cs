using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Event_V3 : MonoBehaviour
{
    [SerializeField] PlayableAsset timeline;
    [SerializeField] UnityEvent events;

    [SerializeField] float delay;

    public void InvokeEvent(out PlayableAsset p)
    {
        //タイムラインがない場合、UnityEventを実行
        if(timeline == null)
        {
            //UnityEventのDelay
            Invoke("Delay", delay);
            p = null;
        }
        //ある場合、タイムラインを返還する
        else
        {
            p = timeline;
        }
    }
    void Delay()
    {
        events.Invoke();
    }
}
