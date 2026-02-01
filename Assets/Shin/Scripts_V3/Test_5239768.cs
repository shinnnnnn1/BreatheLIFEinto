using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Scene/TestC
public class Test_5239768 : MonoBehaviour
{
    NavMeshAgent agent;

    public Transform goal;
    public Vector3 vel;
    public float spd;

    public bool pathPending;


    [SerializeField] Animator anim;
    [SerializeField] Transform[] pos;
    bool isMoving;


    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        //agent.destination = goal.position;

        //_StartMove();
        StartCoroutine(Moving2());
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

    public void StartMove()
    {
        StartCoroutine(Moving2());
    }

    IEnumerator Moving2()
    {
        isMoving = true;


        for(int i = 0; i < pos.Length; i++)
        {
            agent.destination = pos[i].position;
            yield return null;

            while (agent.enabled && isMoving && agent.remainingDistance > agent.stoppingDistance)
            {
                Debug.Log("Moving " + i);
                yield return null;
            }
        }

        yield return null;
        isMoving = false;
        Debug.Log("Finisheeeeeeeeeeeeeeeeee");
    }
}