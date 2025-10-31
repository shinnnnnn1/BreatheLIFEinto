using Unity.Cinemachine;
using UnityEngine;

public class PlayerZoom : MonoBehaviour
{
    [SerializeField] CinemachinePositionComposer positionComposer;
    [SerializeField] PlayerController controller;

    [SerializeField] float minValue;
    [SerializeField] float maxValue;
    [SerializeField] float currentValue;
    [SerializeField] float targetValue;
    [SerializeField] float speed;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        positionComposer.CameraDistance = currentValue;
    }

    void Update()
    {
        if(true)
        {
            targetValue -= controller.zoomDirection.y;

            targetValue = Mathf.Clamp(targetValue, minValue, maxValue);

            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * speed);


            positionComposer.CameraDistance = currentValue;
        }
    }
}
