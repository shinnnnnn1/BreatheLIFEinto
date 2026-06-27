using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region SINGLETON
    private static GameManager instance;
    public static GameManager Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField] float sceneChangeDelay = 1.0f;
    public bool[] canPlay = { true, false, false };

    int nextScene;
    [SerializeField] float resetTime;
    [SerializeField] bool canReset = true;
    public float reset = 60;
 
    public void ChangeScene(int sceneNum)
    {
        nextScene = sceneNum;
        Invoke("LoadScene", sceneChangeDelay);
    }
    void LoadScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void SetCanPlay(int scene)
    {
        canPlay[scene + 1] = true;
    }

    private void Start()
    {
        /*
        if (canPlay[1])
        {
            Debug.Log("Skip");
            Title t = FindFirstObjectByType<Title>();
            t?.SkipTitle();
        }
        */
        //Application.targetFrameRate = 30;
    }
    private void Update()
    {
        /*
        if(canReset)
        {
            resetTime += Time.deltaTime;
            if (resetTime > reset)
            {
                resetTime = 0;
                _RestartGame();
                _RestartScene(0);
                canReset = false;
            }
        }
        */
    }
    public void _SetReset(bool b)
    {
        canReset = b;
        resetTime = 0;
    }

    public void AddReset(float v)
    {
        reset += v;
    }

    public void _RestartScene(int newScene)
    {
        SceneManager.LoadScene(newScene);
    }

    public void _RestartGame()
    {
        canPlay[0] = true;
        canPlay[1] = false;
        canPlay[2] = false;
        canPlay[3] = false;
    }
}
