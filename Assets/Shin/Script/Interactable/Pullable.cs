using UnityEngine;
using UnityEngine.ProBuilder;
using DG.Tweening;
using UnityEngine.Rendering;

public class Pullable : MonoBehaviour, IInteractable
{
    [SerializeField] bool isActivated;
    [SerializeField] bool isPulling;
    [SerializeField] [Range(0, 5)] float pullValue;

    [SerializeField] Vector3 position;
    
    [SerializeField] Vector2 direction;


    float defaultValue;

    void Start()
    {
        defaultValue = pullValue;
    }

    void Update()
    {
        if (isPulling && !isActivated)
        {
            if(GameManager.Instance.player.inputDirection != Vector2.zero)
            {
                Vector3 angle = direction.normalized - GameManager.Instance.player.inputDirection;
                if (angle.magnitude < 1)
                {
                    float value = 1 - angle.magnitude;
                    pullValue -= value * Time.deltaTime;
                    if(pullValue < 0.01f)
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
        isPulling = true;
        GameManager.Instance.player.JoindAdjustment(true);
        GameManager.Instance.player.transform.DOMove(position, 1)
            .OnComplete(()=>GameManager.Instance.player.JoindAdjustment(false));
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
    }
}
