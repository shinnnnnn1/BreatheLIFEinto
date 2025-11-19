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

    [SerializeField] Vector2 zoom_Min_Max;
    [SerializeField] Vector3 zoom_Current_Target_Speed;

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
