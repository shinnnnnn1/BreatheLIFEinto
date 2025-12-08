using UnityEngine;

public class MyScene : MonoBehaviour
{
    private static MyScene instance;
    public static MyScene Instance => instance;

    void Awake()
    {
        Debug.Log(gameObject.name);
        instance = this;
    }

    public void ChangeScene(int sceneNum)
    {;
        GameManager.Instance.ChangeScene(sceneNum);
    }
}
