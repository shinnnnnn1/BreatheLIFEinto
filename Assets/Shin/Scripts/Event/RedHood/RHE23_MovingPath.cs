using UnityEngine;
using DG.Tweening;
using System.Collections;

public class RHE23_MovingPath : MonoBehaviour
{
    [SerializeField] Transform trans, stand;
    [Space(10f)]
    [SerializeField] Animator anim;
    [SerializeField] string movingAnim, stoppedAnim;
    [Space(10f)]
    [SerializeField] Vector3[] paths;
    [SerializeField] float time;
    [Space(10f)]
    [SerializeField] float[] turnTime;
    [SerializeField] bool isRight;
    [SerializeField] bool afterIsRight;

    public void MovingPath()
    {
        anim.SetTrigger(movingAnim);
        trans.DOPath(paths, time, PathType.CatmullRom);
        StartCoroutine(TurnCoroutine());
    }

    IEnumerator TurnCoroutine()
    {
        foreach(float t in turnTime)
        {
            yield return new WaitForSeconds(t);
            Debug.Log("Turn");
            float turnValue = isRight ? -180 : 180;
            stand.DORotate(new Vector3(0, turnValue, 0), 0.2f).SetEase(Ease.Linear).SetRelative();
            isRight = !isRight;
        }
        trans.GetComponent<PlayerController>().model.isRight = afterIsRight;
        anim.SetTrigger(stoppedAnim);
    }
}
