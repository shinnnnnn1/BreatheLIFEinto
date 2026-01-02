using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class PullableObject_V3 : BookDirectional, IInteractable, IPullable
{
    [SerializeField] UnityEvent onEnter, onExit, tryActivate, onPull, onActivate, onResume ,onStop;
    [SerializeField] Animator anim;
    [SerializeField] BookController_V3 bookController;
    PlayerController_V3 player;

    [Space(10f)]
    //잡고 당기는 방향
    [SerializeField] Vector2 direction = new Vector2(1, 0);
    //잡았을때 캐릭터의 목표 위치
    [SerializeField] Vector3 position = new Vector3(1, 0, 0);

    //X축 움직임인지 Z축 움직임인지
    [SerializeField] bool isDirX;
    //+움직임인지 -움직임인지
    [SerializeField] bool isDirPositive;

    [SerializeField] float towardMult = 1.0f;
    [SerializeField] float spring = 1.0f;

    [Space(10f)]
    //얼마나 잡아당겨야하는지. 0이 되면 이벤트 실행
    [SerializeField][Range(0, 5)] float pullValue = 3.0f;
    float defaultPullValue;

    [Space(10f)]
    //당기는 각도의 정확도
    [SerializeField] float angleAccuracy;
    //현재 장력
    [SerializeField] float currentTension;
    [SerializeField] Vector3 tensionDir;

    [Space(10f)]
    [SerializeField] bool isEntered;
    [SerializeField] bool isActivated;
    [SerializeField] bool canPull;

    [SerializeField] bool isPulling;

    void Start()
    {
        defaultPullValue = pullValue;
        //coll = GetComponent<BoxCollider>();
        //CheckDirectional();
    }

    public void OnEnter(bool isRight)
    {
        if (isActivated) { return; }

        if(!isEntered && isRight == direction.x < 0)
        {
            onEnter.Invoke();
            isEntered = true;
        }
    }
    public void OnExit()
    {
        if (isActivated) { return; }

        onExit.Invoke();
        isEntered = false;
    }

    public void OnActivate(PlayerController_V3 p, bool isRight)
    {
        player ??= p;

        //방향이 맞지 않거나 (상하이동일땐 상관 없음) 이미 실행 된 이벤트라면 리턴
        float dir = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? direction.x : 0;
        if ((isDirPositive == isRight && isDirX) || isActivated) { return; }

        //플레이어의 목표 위치를 확인
        Debug.Log(player.transform.position - transform.position);

        //목표 위치와 플레이어의 거리를 통해 이동 시간을 계산
        Vector3 playerPos = player.transform.position;
        Vector3 targetPos = transform.position + position;
        float time = Vector3.Distance(targetPos, playerPos) * towardMult;

        //DOTWEEN으로 플레이어를 이동시킴. 이때는 걷는 모션을 재생시킴.
        //(리틀나이트메어도 목표를 향해 회전, 이동을 함. 비슷한 느낌)
        //목표에 도달했다면 당기는 모션으로 바뀌며 당기기 가능.
        if (Vector3.Distance(playerPos, targetPos) < 0.05f)
        {
            SetCanPull();
        }
        else
        {
            player.transform.DOPause();
            player.SetPlayerAnimation("Walk");
            player.transform.DOLocalMove(targetPos, time).SetEase(Ease.Linear).OnComplete(SetCanPull);
        }
        tryActivate.Invoke();
    }

    public void SetCanPull()
    {
        //당길 수 있는 상태로 설정
        canPull = true;

        //당겨지는 오브젝트의 애니메이션을 활성화하고 속도를 0으로 설정
        anim.enabled = true;
        anim.speed = 0;
        //당겨지는 오브젝트의 애니메이션의 재생
        player.SetPlayerAnimation("IsPulling", true);

        //플레이어의 이동을 제한
        player.SetConstraints(isDirX);
        //캐릭터의 당기는 애니메이션을 재생
        player.SetPlayerAnimation("StartHold");

        onPull.Invoke();

        player.transform.position = transform.position + position;
    }

    public void OnDeactivate(bool isRight)
    {
        //방향이 맞지 않거 리턴
        float dir = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? direction.x : 0;
        if (isDirPositive == isRight && isDirX) { return; }

        //당기는 수치를 초기화.
        canPull = false;
        pullValue = defaultPullValue;
        currentTension = 0;

        //실행되지 않고 놓았다면 애니메이션을 초기화
        if (!isActivated)
        {
            anim.speed = 1;
            anim.SetTrigger("Reset");

            isEntered = false;
            OnEnter(direction.x < 0);
        }

        //두트윈 취소
        player.transform.DOPause();
        //캐릭터의 물리를 초기화
        player.SetConstraints();
        player.SetTension(Vector3.zero);
    }

    void Update()
    {
        if (canPull && !isActivated)
        {
            angleAccuracy = (direction.normalized - player.moveDirection).magnitude;
            player.SetAngleAccuracy(angleAccuracy);

            if (player.moveDirection != Vector2.zero && angleAccuracy < 1f)
            {
                if(!isPulling)
                {
                    onResume.Invoke();
                    isPulling = true;
                }

                anim.speed = 1 - angleAccuracy;

                //張力を計算し、Playerに渡す
                currentTension = transform.position.x + position.x - player.transform.position.x;

                Vector3 abs = new Vector3(Mathf.Abs(direction.x), 0, Mathf.Abs(direction.y));
                tensionDir = abs * currentTension * spring;

                player.SetTension(tensionDir);

                //引っ張ってイベントを発生
                pullValue -= (1 - angleAccuracy) * Time.deltaTime;
                if (pullValue < 0.01f)
                {
                    isActivated = true;
                    anim.speed = 1;
                    player.ActionCancel();
                    player.SetCanMove(false);

                    onActivate.Invoke();
                }
            }
            else
            {
                if (isPulling)
                {
                    onStop.Invoke();
                    isPulling = false;
                }
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

    /*
    public void CheckDirectional()
    {
        if (isDirectional[bookController.bookDir + 2])
        {
            //coll.enabled = true;
        }
        else
        {
            //coll.enabled = false;
        }
    }
    */
}
