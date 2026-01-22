using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.AI;

public class HGE15_NavMeshChase : MonoBehaviour
{
    [SerializeField] Transform chasingObj;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] PlaneObject npc;
    [SerializeField] Animator anim;
    [SerializeField] Collider coll;
    public bool isChasing;
    [SerializeField] bool isCompleted;

    private void Update()
    {
        if(!isCompleted)
        {
            if (agent.enabled)
            {
                if (isChasing)
                {
                    agent.destination = chasingObj.position;
                    npc.AutoTurn(agent.velocity);

                    string animTrigger = agent.velocity.magnitude > 0 ? "Walk" : "Idle";
                    Debug.Log(animTrigger);
                    anim.SetTrigger(animTrigger);
                }
                else
                {
                   
                    agent.destination = agent.transform.position;
                    anim.SetTrigger("Idle");
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        isChasing = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isChasing = false;
    }
    public void _IsCompleted()
    {
        isCompleted = true;
        agent.enabled = false;
    }
}
