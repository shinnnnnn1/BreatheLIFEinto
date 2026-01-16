using UnityEngine;

public class HGE12_OutlineFlash : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] string parameterName = "_IsFlashing";
    [SerializeField] bool defaultValue = true;

    private void OnDestroy()
    {
        mat.SetFloat(parameterName, defaultValue ? 1f : 0f);
    }

    public void _SetParam(bool isOn)
    {
        mat.SetFloat(parameterName, isOn ? 1f : 0f);
    }
}
