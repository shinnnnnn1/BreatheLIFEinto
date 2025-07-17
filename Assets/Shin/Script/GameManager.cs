using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public PlayableAsset[] timelines;
    PlayableDirector director;

    public PlayerCtrl player;
    public Book book;
    public PageTrigger trigger;

    public PhysicsMaterial[] hMat;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void PlayCutScene(int number)
    {
        Debug.Log($"Cut {number} Start");
        director.playableAsset = timelines[number];
        director.Play();
    }
}
