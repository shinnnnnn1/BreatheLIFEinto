using UnityEngine;
using DG.Tweening;

public class PullableObject : MonoBehaviour, IInteractable, IPullable
{
    PlayerController player;
    [SerializeField] Animator anim;

    //X축 움직임인지 Z축 움직임인지
    [SerializeField] bool isDirX = true;

    //+움직임인지 -움직임인지
    [SerializeField] bool isDirPositive = false;

    [SerializeField] Vector2 direction = new Vector2(1, 0);
    [SerializeField] Vector3 position = new Vector2(1, 0);
    [SerializeField] float travelSpeedMultiplier = 1.0f;
    [SerializeField] float spring = 1.0f;

    [Space(10f)]
    [SerializeField][Range(0, 5)] float pullValue = 3.0f;
    float defaultPullValue;

    [Space(10f)]
    [SerializeField] float angleAccuracy;
    [SerializeField] float currentTension;
    [SerializeField] Vector3 tensionDir;

    [Space(10f)]
    [SerializeField] bool isActivated = false;
    bool canPull = false;

    void Start()
    {
        defaultPullValue = pullValue;
    }

    public void OnEnter()
    {

    }
    public void OnExit()
    {

    }

    public void OnActivate(PlayerController p, bool isRight)
    {
        player ??= p;

        //방향이 맞지 않거나 이미 실행 된 이벤트라면 리턴
        if (isDirPositive == isRight || isActivated) { return; }

        //플레이어의 목표 위치를 확인
        //Debug.Log(player.transform.position - transform.position);

        //목표 위치와 플레이어의 거리를 통해 이동 시간을 계산
        Vector3 playerPos = player.transform.position;
        Vector3 targetPos = transform.position + position;
        float time = Vector3.Distance(targetPos, playerPos) * travelSpeedMultiplier;

        //DOTWEEN으로 플레이어를 이동시킴. 이때는 걷는 모션을 재생시킴.
        //(리틀나이트메어도 목표를 향해 회전, 이동을 함. 비슷한 느낌)
        //목표에 도달했다면 당기는 모션으로 바뀌며 당기기 가능.
        player.view.SetPlayerAnim("VelocityX", 1.0f);
        player.transform.DOLocalMove(targetPos, time).SetEase(Ease.Linear).OnComplete(SetCanPull);
    }

    public void SetCanPull()
    {
        canPull = true;

        anim.enabled = true;
        anim.speed = 0;

        anim.SetTrigger("StartAnim");

        player.SetConstraints(isDirX, isDirPositive);

        player.view.SetPlayerAnim("StartHold");
        player.view.SetPlayerAnim("IsPulling", true);
    }

    public void OnDeactivate()
    {
        //당기는 수치를 초기화.
        canPull = false;
        pullValue = defaultPullValue;
        currentTension = 0;

        if(!isActivated)
        {
            anim.speed = 1;
            anim.SetTrigger("Reset");
        }

        //두트윈 취소.
        player.transform.DOPause();

        player.SetConstraints();
        player.SetTension(Vector3.zero);
        //player.SetCanAnim(true);
    }

    void Update()
    {
        if(canPull && !isActivated)
        {
            angleAccuracy = (direction.normalized - player.moveDirection).magnitude;
            player.SetAngleAccuracy(angleAccuracy);

            if (player.moveDirection != Vector2.zero && angleAccuracy < 1f)
            {
                anim.speed = 1 - angleAccuracy;

                //張力を計算し、Playerに渡す
                currentTension = transform.position.x + position.x - player.transform.position.x;
                tensionDir = -direction * currentTension * spring;

                player.SetTension(tensionDir);

                //引っ張ってイベントを発生
                pullValue -= (1 - angleAccuracy) * Time.deltaTime;
                if (pullValue < 0.01f)
                {
                    isActivated = true;
                    anim.speed = 1;
                    player.ActionCancel();
                    player.SetCanMove(false);
                    EventManager.Instance.PlayCutScene(1);
                }
            }
            else
            {
                anim.speed = 0;
                player.SetTension(Vector3.zero);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, new Vector3(direction.x, 0, direction.y));

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + position, 0.1f);
    }
}
