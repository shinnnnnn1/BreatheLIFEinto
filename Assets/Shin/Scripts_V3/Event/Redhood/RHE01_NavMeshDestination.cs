using UnityEngine;
using UnityEngine.AI;

public class RHE01_NavMeshDestination : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Vector3 destination;

    public void StartMove()
    {
        agent.destination = destination;
    }
}
