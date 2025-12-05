using System.Collections;
using UnityEngine;

public class HGE03_MatFade : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] string parameterName;
    [SerializeField] float defaultValue;
    [SerializeField] float goal;
    [SerializeField] float speed = 1f;
    [SerializeField] bool isAdd;

    [SerializeField] float currentValue;

    void Awake()
    {
        _SetParam(defaultValue);
        currentValue = mat.GetFloat(parameterName);
    }

    public void _SetParam(float value)
    {
        mat.SetFloat(parameterName, value);
    }

    public void _StartFade()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        if(isAdd)
        {
            if(currentValue < goal)
            {
                currentValue += Time.deltaTime * speed;
                mat.SetFloat(parameterName, currentValue);
                yield return null;
            }
        }
        else
        {
            if (currentValue > goal)
            {
                currentValue -= Time.deltaTime * speed;
                mat.SetFloat(parameterName, currentValue);
                yield return null;
            }
        }
    }
}
