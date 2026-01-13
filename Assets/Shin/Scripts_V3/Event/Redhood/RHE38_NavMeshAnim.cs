using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;

public class RHE38_NavMeshAnim : MonoBehaviour
{
    [SerializeField] PlayerController_V3 player;
    [SerializeField] PlaneObject npc;

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Vector3 pos;
    bool isMoving;

    [SerializeField] UnityEvent onEnd;

    public void _StartMove()
    {
        StartCoroutine(Moving());
    }

    IEnumerator Moving()
    {
        isMoving = true;
        anim.SetTrigger("Walk");

        agent.destination = pos;
        yield return null;

        while (agent.enabled && isMoving && agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        yield return null;
        anim.SetTrigger("Idle");
        isMoving = false;

        onEnd.Invoke();
    }

    void Update()
    {
        if(isMoving)
        {
            if(player != null)
            {
                player.AutoTurn(agent.velocity);
            }
            else if(npc != null)
            {
                npc.AutoTurn(agent.velocity);
            }
        }
    }

    public void _StopMoving()
    {
        isMoving = false;
        //StopCoroutine(Moving());
        npc.SetBone();
    }

    public void _ForceStop()
    {
        StopCoroutine(Moving());
        isMoving = false;
    }
}
