using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class HGE03_GrabFog : MonoBehaviour
{
    [SerializeField] VirtualMouseController controller;
    [SerializeField] Transform model;

    [SerializeField] Vector2 point;

    [SerializeField] [Range(0, 10)] float distance;
    [SerializeField] [Range(0, 10)] float goal = 5.0f;
    [SerializeField] bool isGrab, isActivated;

    [SerializeField] Vector2 startPoint;
    [SerializeField] Vector2 movedPoint;

    Vector2 previoutPoint;

    [SerializeField] UnityEvent onActivate, onActivateDelay;
    [SerializeField] float delay;

    private void Start()
    {
        startPoint = transform.position;
        model.DOShakePosition(30, 0.1f, 1, 90, false, false).SetLoops(-1);
    }

    public void SetGrab(bool startGrab)
    {
        isGrab = startGrab;

        if(startGrab)
        {
            model.DOPause();
            point = controller.hitPoint;
            previoutPoint = controller.hitPoint;
        }
        else
        {
            point = Vector3.zero;
            previoutPoint = Vector3.zero;
            model.DOShakePosition(30, 0.1f, 1, 90, false, false).SetLoops(-1);
        }
    }

    private void FixedUpdate()
    {
        if (!isActivated)
        {
            if (isGrab)
            {
                point = controller.hitPoint;

                Vector2 a = point - previoutPoint;

                model.position = model.position + (Vector3)a;

                previoutPoint = point;

                movedPoint = (Vector2)transform.position - startPoint;
            }

            distance = movedPoint.magnitude;

            if (distance >= goal)
            {
                onActivate.Invoke();
                Invoke("ActivateDelay", delay);
                isActivated = true;
            }
        }
    }

    void ActivateDelay()
    {
        onActivateDelay.Invoke();
    }
}
