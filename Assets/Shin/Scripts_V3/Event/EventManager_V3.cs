using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// イベントの実行はUnityEventとTimelineに分かれる。
/// UnityEventの場合、Event_V3スクリプトでそのまま実行。
/// Timelineの場合、PlayableDirectorはここにあるため、Timelineをここに持ってきて再生する。
/// </summary>
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
    [SerializeField] FadeUI[] ui;
    PlayableDirector director;

    void Start()
    {
        //全てのEvent_V3を参照
        events = GetComponentsInChildren<Event_V3>();
        //PlayableDirectorを参照
        director = GetComponent<PlayableDirector>();
    }

    public void InvokeEvent(int number)
    {
        Debug.Log($"Event {number} Start");

        if (events.Length > number)
        {
            //number番目のEvent_V3を実行する
            events[number].InvokeEvent(out PlayableAsset p);

            //Timelineの場合、outにTimelineが入ってる
            if (p != null)
            {
                //もらったTimelineを入れて再生する
                director.playableAsset = p;
                director.Play();
            }
        }
    }

    public void FadeAllUI(bool fadeIn)
    {
        float value = fadeIn ? 1f : 0f;
        foreach(FadeUI img in ui)
        {
            img.Fade(value);
        }
    }
    public void FadeOneUI(bool fadeIn, int num)
    {
        float value = fadeIn ? 1f : 0f;
        ui[num].Fade(value);
    }
    public void FadeInUI(int num)
    {
        ui[num].Fade(1);
    }
    public void FadeOutUI(int num)
    {
        ui[num].Fade(0);
    }

    public void _RemoveTimeline() => director.playableAsset = null;
}
