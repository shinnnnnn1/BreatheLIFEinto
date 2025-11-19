using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class Event_V3 : MonoBehaviour
{
    //基本的にTimelineがあるかどうかで判断する
    [SerializeField] PlayableAsset timeline;
    [SerializeField] UnityEvent events;
    [SerializeField] float delay;

    public void InvokeEvent(out PlayableAsset p)
    {
        //UnityEventの場合、Delayの後に実行
        if(timeline == null)
        {
            //UnityEventのDelay
            Invoke("Delay", delay);

            //Timelineには何も入れない
            p = null;
        }
        //Timelineの場合、Timelineを渡す
        else
        {
            p = timeline;
        }
    }
    void Delay()
    {
        //UnityEventの実行
        events.Invoke();
    }
}
