using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class EventManager_V3 : MonoBehaviour
{
    #region SINGLETON
    private static EventManager_V3 instance;
    public static EventManager_V3 Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

    [SerializeField] Event_V3[] events;

    PlayableDirector director;

    void Start()
    {
        events = GetComponentsInChildren<Event_V3>();
        director = GetComponent<PlayableDirector>();
    }

    public void InvokeEvent(int number)
    {
        //イベントを実行
        events[number].InvokeEvent(out PlayableAsset p);

        //タイムラインの場合、タイムラインをもらって実行する
        if(p != null)
        {
            director.playableAsset = p;
            director.Play();
        }
    }
}
