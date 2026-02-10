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
