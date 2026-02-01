using UnityEngine;
using UnityEngine.Events;

public class HoldableObject_V3 : MonoBehaviour, IInteractable
{
    [SerializeField] PlayerController_V3 player;
    [SerializeField] UnityEvent onEnter, onExit, onPullStart, onPullEnd
        , onResume, onStop;
    [SerializeField] bool isEntered, isPulling, isResumed;

    public void OnEnter(bool isRight)
    {
        if (!isEntered)
        {
            onEnter.Invoke();
            isEntered = true;
        }
    }
    public void OnExit()
    {
        onExit.Invoke();
        isEntered = false;
    }
    public void OnPullStart()
    {
        onPullStart.Invoke();
        isPulling = true;
    }
    public void OnPullEnd()
    {
        onPullEnd.Invoke();
        onStop.Invoke();
        isPulling = false;
    }

    void Update()
    {
        if (isPulling)
        {
            if (player.moveDirection.magnitude > 0.1f && !isResumed)
            {
                isResumed = true;
                onResume.Invoke();
            }
            else if (player.moveDirection.magnitude < 0.1f && isResumed)
            {
                isResumed = false;
                onStop.Invoke();
            }
        }
    }
}
