using System.Collections;
using UnityEngine;

public class RHE27_WolfLookAround : MonoBehaviour
{
    [SerializeField] Transform wolf;
    [SerializeField] float time;

    public void LookAround()
    {
        StartCoroutine(Look());
    }
    IEnumerator Look()
    {
        wolf.transform.eulerAngles = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(time);
        wolf.transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
