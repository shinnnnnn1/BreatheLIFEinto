using UnityEngine;
using UnityEngine.Playables;

public class HGE20_TimelineEvent : MonoBehaviour
{
    [SerializeField] PlayableDirector director;

    public void _StartTimelineEvent()
    {
        director.time = 0;
        director.Play();
    }
    public void _StopTimelineEvent()
    {
        director.Stop();
    }
}
