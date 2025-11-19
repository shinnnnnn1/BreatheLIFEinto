using UnityEngine;
using UnityEngine.AI;

// Scene/TestC
public class Test_5239768 : MonoBehaviour
{
    public Transform goal;

    void OnEnable()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
    }
}
