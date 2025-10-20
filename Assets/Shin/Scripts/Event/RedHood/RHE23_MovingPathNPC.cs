using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RHE23_MovingPathNPC : MonoBehaviour
{
    [SerializeField] NPCObject trans;
    [Space(10f)]
    [SerializeField] Animator anim;
    [SerializeField] string movingAnim, stoppedAnim;
    [Space(10f)]
    [SerializeField] Vector3[] paths;
    [SerializeField] float time;
    [Space(10f)]
    [SerializeField] float[] turnTime;

    public void MovingPath()
    {
        anim.SetTrigger(movingAnim);
        trans.transform.DOPath(paths, time, PathType.CatmullRom)
            .SetEase(Ease.Linear).OnComplete( ()=> { anim.SetTrigger(stoppedAnim); });
        StartCoroutine(TurnCoroutine());
    }

    IEnumerator TurnCoroutine()
    {
        foreach(float t in turnTime)
        {
            yield return new WaitForSeconds(t);
            trans.Turn();
        }
    }
}
