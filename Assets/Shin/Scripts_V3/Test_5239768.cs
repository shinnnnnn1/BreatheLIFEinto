using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

// Scene/TestC
public class Test_5239768 : MonoBehaviour
{
    NavMeshAgent agent;

    public Transform goal;
    public Vector3 vel;
    public float spd;

    public bool pathPending;


    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = goal.position;

        _StartMove();
    }

    private void Update()
    {
        //vel = agent.velocity;
        //spd = vel.magnitude;

        //pathPending = agent.remainingDistance > agent.stoppingDistance;
    }

    public void _StartMove()
    {
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        agent.destination = goal.position;
        yield return null;

        while (agent.remainingDistance > agent.stoppingDistance)
        {
            Debug.Log("asdasdasdasdsad");
            yield return null;
        }
        Debug.Log("End");
    }

}
