using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RHE22_Revive : MonoBehaviour
{
    [SerializeField] NPCObject grand;
    [SerializeField] NPCObject redhood;
    [SerializeField] NPCObject hunter;

    [SerializeField] Animator grandA;
    [SerializeField] Animator redhoodA;

    [SerializeField] float firstTurn;
    [SerializeField] float turnTime;

    public void WakeUp()
    {
        StartCoroutine(WakeUpCoroutine());
    }

    IEnumerator WakeUpCoroutine()
    {
        hunter.TurnToPlayer();

        grand.TurnAndChangeToPlayer();
        redhood.TurnAndChangeToPlayer();

        yield return new WaitForSeconds(0.1f);

        grandA.SetTrigger("Idle");
        redhoodA.SetTrigger("Idle");

    }
}
