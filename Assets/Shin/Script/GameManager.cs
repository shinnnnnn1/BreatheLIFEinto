using UnityEngine;
using UnityEngine.InputSystem;
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
    public GamepadCursor gamepadCursor;

    PlayerInput characterInput, playerInput;

    public PhysicsMaterial[] hMat;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();

        characterInput = player.GetComponent<PlayerInput>();
        playerInput = gamepadCursor.GetComponent<PlayerInput>();
    }

    public void PlayCutScene(int number)
    {
        Debug.Log($"Cut {number} Start");
        director.playableAsset = timelines[number];
        //director.Play();
    }

    public void Switch(bool characterToPlayer)
    {
        if(characterToPlayer)
        {
            characterInput.enabled = false;
            //playerInput.enabled = true;
            gamepadCursor.gameObject.SetActive(true);
        }
        else
        {
            gamepadCursor.gameObject.SetActive(false);
            characterInput.enabled = true;
        }
    }
}
