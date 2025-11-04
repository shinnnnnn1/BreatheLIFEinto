using DG.Tweening;
using UnityEngine;

public class PlayerView_V3 : MonoBehaviour
{
    [SerializeField] Transform stand;
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

    public void Turn(bool isRight, float turnTime)
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

    public void StandFlip(float value, bool isOpen)
    {
        stand.DOLocalRotate(new Vector3(-value, 0, 0), 1.2f).SetRelative()
            .OnComplete(() => stand.gameObject.SetActive(isOpen));
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

    public void SetPlayerVisible(bool isVisible)
    {
        stand.gameObject.SetActive(isVisible);
    }

    public void SetPlayerAnim(string name, float value)
    {
        anim.SetFloat(name, value);
    }
    public void SetPlayerAnim(string name, bool value)
    {
        anim.SetBool(name, value);
    }
    public void SetPlayerAnim(string name)
    {
        anim.SetTrigger(name);
    }
}
