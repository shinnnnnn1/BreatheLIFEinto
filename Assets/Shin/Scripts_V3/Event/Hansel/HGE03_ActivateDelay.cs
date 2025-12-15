using UnityEngine;

public class HGE03_ActivateDelay : MonoBehaviour
{
    public void _OnActivate(float delay)
    {
        Invoke("Act", delay);
    }
    void Act()
    {
        gameObject.SetActive(true);
    }
}
