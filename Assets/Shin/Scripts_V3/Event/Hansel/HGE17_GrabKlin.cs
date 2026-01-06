using DG.Tweening;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class HGE17_GrabKlin : MonoBehaviour
{
    [SerializeField] VirtualMouseController controller;
    [SerializeField] Transform model;

    [SerializeField] Vector2 point;

    [SerializeField] bool isGrab, isActivated;

    [SerializeField] Vector2 startPoint;
    [SerializeField] Vector2 movedPoint;

    Vector2 previoutPoint;

    [SerializeField] UnityEvent onActivate, onActivateDelay;
    [SerializeField] float delay;

    private void OnEnable()
    {
        startPoint = model.transform.position;
    }

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
            model.position = startPoint;
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

                model.position = model.position + (Vector3)a;

                previoutPoint = point;

                movedPoint = (Vector2)transform.position - startPoint;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated)
        {
            isActivated = true;
            onActivate.Invoke();
        }
    }
}
