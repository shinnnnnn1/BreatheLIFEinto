using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class EventManager : MonoBehaviour
{
    private static EventManager instance;
    public static EventManager Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public PlayableAsset[] timelines;
    PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void PlayCutScene(int number)
    {
        Debug.Log($"Scene {number} Start");
        director.playableAsset = timelines[number];
        director.Play();
    }
}
