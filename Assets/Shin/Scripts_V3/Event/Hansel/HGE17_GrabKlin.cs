using DG.Tweening;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

public class HGE17_GrabKlin : MonoBehaviour
{
    [SerializeField] VirtualMouseController controller;
    [SerializeField] Transform model;

    [SerializeField] Vector2 point;
    Vector2 previoutPoint;

    [SerializeField] bool isGrab, isActivated;

    [Space(10)]
    [SerializeField] float activateValue = 1;

    [SerializeField] UnityEvent onActivate;

    public void SetGrab(bool startGrab)
    {
        if (isActivated) { return; }

        isGrab = startGrab;

        if (startGrab)
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

    private void Update()
    {
        if (!isActivated)
        {
            if (isGrab)
            {
                point = controller.hitPoint;

                Vector2 a = point - previoutPoint;
                Debug.Log(a.magnitude);

                if(a.magnitude > activateValue)
                {
                    isActivated = true;
                    onActivate.Invoke();
                }
            }
        }
    }
}
