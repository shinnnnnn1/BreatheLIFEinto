using UnityEngine;

public class BookDirectional : MonoBehaviour, IBookDirectional
{
    public bool[] enabledDirections = new bool[] { false, false, true, false, false };
    public Collider colli;

    void Awake()
    {
        if(colli == null)
        {
            colli = GetComponent<Collider>();
        }
        OnCheckDirectional(2);
    }
    public void OnCheckDirectional(int bookDirection)
    {
        if(gameObject.activeSelf)
        {
            colli.enabled = enabledDirections[bookDirection];
        }
    }
}
