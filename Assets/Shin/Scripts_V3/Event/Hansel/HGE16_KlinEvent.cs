using UnityEngine;

public class HGE16_KlinEvent : MonoBehaviour
{
    [SerializeField] HGE15_NavMeshChase chaseEvent;
    [SerializeField] Collider coll;

    private void Update()
    {
        coll.enabled = chaseEvent.isChasing;
    }
}
