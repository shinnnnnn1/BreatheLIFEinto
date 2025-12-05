using UnityEngine;
using UnityEngine.Events;

public class HGE03_GrabFog : MonoBehaviour
{
    [SerializeField] VirtualMouseController controller;
    [SerializeField] Transform model;

    [SerializeField] Vector2 point;

    [SerializeField] [Range(0, 10)] float distance;
    [SerializeField] [Range(0, 10)] float goal = 5.0f;
    [SerializeField] bool isGrab;

    [SerializeField] Vector2 startPoint;
    [SerializeField] Vector2 movedPoint;

    Vector2 previoutPoint;

    [SerializeField] UnityEvent onActivate;

    private void Start()
    {
        startPoint = transform.position;
    }

    public void SetGrab(bool startGrab)
    {
        isGrab = startGrab;

        if(startGrab)
        {
            point = controller.hitPoint;
            previoutPoint = controller.hitPoint;
        }
        else
        {
            point = Vector3.zero;
            previoutPoint = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if(isGrab)
        {
            point = controller.hitPoint;

            Vector2 a = point - previoutPoint;

            model.position = model.position + (Vector3)a;

            previoutPoint = point;

            movedPoint = (Vector2)transform.position - startPoint;
        }

        distance = movedPoint.magnitude;

        if(distance >= goal)
        {
            onActivate.Invoke();
        }
    }

}
