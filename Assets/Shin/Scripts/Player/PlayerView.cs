using UnityEngine;
using DG.Tweening;

public class PlayerView : MonoBehaviour
{
    Animator anim;
    Rigidbody rigid;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    public void SetLinearVelocity(Vector3 velocity)
    {
        rigid.linearVelocity = new Vector3(velocity.x, rigid.linearVelocity.y, velocity.z);
    }

    public void Turn(Transform stand, bool isRight, float turnTime)
    {
        Vector3 rot = new Vector3(0, isRight ? -180 : 180, 0);
        float time = turnTime;
        stand.DOLocalRotate(rot, turnTime).SetEase(Ease.Linear).SetRelative();
    }

    public void Jump(float jumpPow)
    {
        rigid.linearVelocity = new Vector3(rigid.linearVelocity.x, 0, rigid.linearVelocity.z);
        rigid.AddForce(Vector3.up * jumpPow, ForceMode.Impulse);
    }

    public void StandFlip(Transform stand, float value, bool isStart)
    {
        stand.gameObject.SetActive(true);
        stand.DOLocalRotate(new Vector3(-value, 0, 0), 0.8f).SetRelative()
            .OnComplete(()=> stand.gameObject.SetActive(!isStart));
    }

    public void AdjustmentLocalPosition(Vector3 pos)
    {
        transform.localPosition = pos;
    }
    public void AdjustmentPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void AdjustmentEulerAngles(Vector3 rot)
    {
        transform.eulerAngles = rot;
    }

    public void SetPlayerVisible(Transform stand, bool isVisible)
    {
        stand.gameObject.SetActive(isVisible);
    }
}
