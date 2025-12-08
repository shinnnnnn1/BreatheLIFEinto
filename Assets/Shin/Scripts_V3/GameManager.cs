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
    [SerializeField] bool[] canPlay = { true, false, false };

    int nextScene;

    void OnEnable()
    {
        Debug.Log(gameObject.name);

        // can Play to set interactable book

        //Find Title Script and Method Go if Title Script is this scene.

    }

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
        canPlay[scene] = true;
    }
}
