using UnityEngine;
using UnityEngine.Playables;

public class CurtainReverse : MonoBehaviour
{
    [SerializeField] PlayableDirector director;
    [SerializeField] float startReverse;
    [SerializeField] bool reverse;
    float currentTime;

    void Start()
    {
        currentTime = startReverse;
    }

    public void ReverseCurtain()
    {
        currentTime = startReverse;
        reverse = true;
    }

    void Update()
    {
        if(reverse)
        {
            currentTime -= Time.deltaTime;
            director.time = currentTime;

            if(currentTime <= 0)
            {
                director.time = 0;
                reverse = false;
            }

            director.Evaluate();
        }
    }
}
