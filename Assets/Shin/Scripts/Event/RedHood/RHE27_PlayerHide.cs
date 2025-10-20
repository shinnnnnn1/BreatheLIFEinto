using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RHE27_PlayerHide : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] Transform stand;
    [SerializeField] Animator anim;
    [SerializeField] Vector3 pos;
    [SerializeField] float time;


    public void Playerhide()
    {
        anim.SetTrigger("Move");
        if ((player.model.isRight && player.transform.position.x > pos.x) || 
            (!player.model.isRight && player.transform.position.x < pos.x))
        {
            float value = player.model.isRight ? -180 : 180;
            stand.DORotate(new Vector3(0, value, 0), 0.2f).SetEase(Ease.Linear).SetRelative();
            player.model.isRight = !player.model.isRight;
        }

        player.transform.DOMove(pos, time).SetEase(Ease.Linear).OnComplete(AfterMove);
    }

    void AfterMove()
    {
        if (!player.model.isRight)
        {
            stand.DORotate(new Vector3(0, 180, 0), 0.2f).SetEase(Ease.Linear).SetRelative();
        }
        anim.SetTrigger("Idle");
    }
}
