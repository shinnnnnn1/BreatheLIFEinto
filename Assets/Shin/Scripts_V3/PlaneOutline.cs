using UnityEngine;

public class PlaneOutline : MonoBehaviour
{
    public Material mat;

    public void _SetFloat(float value)
    {
        mat.SetFloat("_Float", value);
    }
}
