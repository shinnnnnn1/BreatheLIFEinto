using UnityEngine;
using UnityEngine.ProBuilder;
using DG.Tweening;
using UnityEngine.Rendering;

public class Pullable : MonoBehaviour, IInteractable
{
    Rigidbody rigid;
    bool isActivated;
    bool isPulling;
    [SerializeField] bool isRight;
    [SerializeField] [Range(0, 5)] float pullValue;
    [SerializeField] float spring;
    [SerializeField] Vector2 direction;
    [SerializeField] Vector3 position;
    float defaultValue;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        defaultValue = pullValue;
    }

    void Update()
    {
        if (isPulling && !isActivated)
        {
            Vector3 angle = direction.normalized - GameManager.Instance.player.inputDirection;
            if (GameManager.Instance.player.inputDirection != Vector2.zero && angle.magnitude < 1)
            {
                float value = 1 - angle.magnitude;
                if (value > 0.9f)
                {
                    pullValue -= value * Time.deltaTime;
                    if (pullValue < 0.01f)
                    {
                        GameManager.Instance.player.canMove = false;
                        isActivated = true;
                        OnDeactivate();

                        Debug.Log("Event");
                    }
                }
            }
            else
            {
                pullValue += Time.deltaTime;
                pullValue = Mathf.Clamp(pullValue, 0, defaultValue);
            }
        }
    }

    public void OnEnter()
    {

    }
    public void OnExit()
    {

    }
    public void OnActivate()
    {
        if(GameManager.Instance.player.isRight == isRight)
        {
            isPulling = true;
            Vector3 pos = transform.position + position;
            GameManager.Instance.player.JointAdjustment(true);
            GameManager.Instance.player.transform.DOMove(pos, 0.2f).SetEase(Ease.OutCubic)
                .OnComplete(() => GameManager.Instance.player.JointAdjustment(false));
        }
    }
    public void OnDeactivate()
    {
        isPulling = false;
        pullValue = defaultValue;
        GameManager.Instance.player.transform.DOPause();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, new Vector3(direction.x, 0, direction.y));

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + position, 0.1f);
    }
}
