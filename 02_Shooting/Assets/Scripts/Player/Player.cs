using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]    // 반드시 특정 컴포넌트가 필요한 경우에 추가
public class Player : MonoBehaviour
{
    // InputManager : 기존의 유니티 입력방식
    //  장점 : 간단하다.
    //  단점 : Busy-wait이 발생할 수 밖에 없다. 인풋랙이 있을 수 있다.

    // InputSystem : 유니티의 새로운 입력 방식
    //  Event-driven 방식 적용.

    PlayerInputActions inputActions;

    /// <summary>
    /// 마지막으로 입력된 방향을 기록하는 변수
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 플레이어의 이동 속도
    /// public 맴버 변수는 인스팩터 창에서 확인이 가능하다.
    /// </summary>
    //[Range(0.0f,1.0f)]    // 스크롤 바를 이용해 값을 조절할 수 있다.
    public float moveSpeed = 0.01f;

    //[SerializeField]      // public이 아닌 경우에도 인스펙터 창에서 확인이 가능해진다.(권장하지 않음)
    //float test = 1.0f;

    Animator anim;
    readonly int InputY_String = Animator.StringToHash("InputY");
    Rigidbody2D rigid2d;
    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 총알의 프리팹
    /// </summary>
    public GameObject bulletPrefab;

    /// <summary>
    /// 총알 발사 위치 지정용
    /// </summary>
    Transform[] fireTransforms;

    /// <summary>
    /// 총알 발사 플래시 이팩트
    /// </summary>
    GameObject fireFlash;

    /// <summary>
    /// 플래시 기다리는 시간
    /// </summary>
    WaitForSeconds flashWait;

    /// <summary>
    /// 연사를 실행할 코루틴
    /// </summary>
    IEnumerator fireCoroutine;

    /// <summary>
    /// 연사 시간 간격
    /// </summary>
    public float fireInterval = 0.5f;

    /// <summary>
    /// 플레이어의 점수
    /// </summary>
    int score = 0;

    /// <summary>
    /// 점수 확인 및 설정용 프로퍼티
    /// </summary>
    public int Score
    {
        get => score;   // 읽기는 public
        private set     // 쓰기는 private
        {
            if( score != value)
            {
                score = Math.Min(value,99999);  // 최대 점수 99999
                onScoreChange?.Invoke(score);   // 이 델리게이트에 함수를 등록한 모든 대상에게 변경된 점수를 알림
            }
        }
    }

    /// <summary>
    /// 점수가 변경되었을 때 알리는 델리게이트(파라메터: 변경된 점수)
    /// </summary>
    public Action<int> onScoreChange;

    /// <summary>
    /// 파워 3단계에서 파워업 아이템을 먹었을 때 얻는 보너스 점수
    /// </summary>
    public int powerBonus = 1000;

    /// <summary>
    /// 최소 파워 단계
    /// </summary>
    private const int MinPower = 1;

    /// <summary>
    /// 최대 파워 단계
    /// </summary>
    private const int MaxPower = 3;

    /// <summary>
    /// 한번에 여러 총알을 쏠 때 총알 간의 간격
    /// </summary>
    private const float FireAngle = 30.0f;

    /// <summary>
    /// 현재 파워
    /// </summary>
    private int power = 1;

    /// <summary>
    /// 파워 확인 및 설정용 프로퍼티
    /// </summary>
    private int Power
    {
        get => power;
        set
        {
            if( power != value) // 변경이 있을 때만 처리
            {
                power = value;
                if( power > MaxPower)
                {
                    AddScore(powerBonus);   // 파워가 최대치를 벗어나면 보너스 점수 추가
                }

                power = Mathf.Clamp(power, MinPower, MaxPower); // 파워는 최소~최대 단계로만 존재

                RefreshFirePositions();     // 총알 발사 위치 조정
            }
        }
    }

    /// <summary>
    /// 플레이어의 남은 생명
    /// </summary>
    private int life = 3;
    
    /// <summary>
    /// 시작시 플레이어의 생명
    /// </summary>
    const int StartLife = 3;

    /// <summary>
    /// 생명을 설정하고 확인하기 위한 프로퍼티
    /// </summary>
    private int Life
    {
        get => life;
        set
        {
            if(life != value)   // 변화가 있을 때만 처리
            {
                life = value;
                if(IsAlive)
                {
                    OnHit();    // 맞고 나서 살아있으면 맞은 처리
                }
                else
                {
                    OnDie();    // 맞고 나서 죽었으면 죽은 처리
                }

                life = Mathf.Clamp(life, 0, StartLife); // 항상 0~3
                onLifeChange?.Invoke(life); // 생명 변화가 있었음을 알림
            }
        }
    }

    /// <summary>
    /// 살았는지 죽었는지 확인하기 위한 프로퍼티
    /// </summary>
    private bool IsAlive => life > 0;

    /// <summary>
    /// 생명에 변화가 있음을 알리는 델리게이트
    /// </summary>
    public Action<int> onLifeChange;

    /// <summary>
    /// 맞았을 때 무적시간
    /// </summary>
    public float invincibleTime = 2.0f;

    /// <summary>
    /// 죽었음을 알리는 델리게이트(점수도 같이 전달)
    /// </summary>
    public Action<int> onDie;


    // 이 스크립트가 포함된 게임 오브젝트가 생성 완료되면 호출된다.
    private void Awake()
    {
        inputActions = new PlayerInputActions();        // 인풋 액션 생성
        anim = GetComponent<Animator>();   // 이 스크립트가 들어있는 게임 오브젝트에서 컴포넌트를 찾아서 anim에 저장하기(없으면 null)
        // null; // 참조가 비어있다.
        rigid2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 게임 오브젝트 찾는 방법
        // GameObject.Find("FirePosition"); // 이름으로 게임 오브젝트 찾기
        // GameObject.FindAnyObjectByType<Transform>(); // 특정 컴포넌트를 가지고 있는 게임 오브젝트 찾기
        // GameObject.FindFirstObjectByType<Transform>();  // 특정 컴포넌트를 가지고 있는 첫번째 게임 오브젝트 찾기
        // GameObject.FindGameObjectWithTag("Player");  // 게임 오브젝트의 태그를 기준으로 찾는 함수
        // GameObject.FindGameObjectsWithTag("Player"); // 특정 태그를 가진 모든 게임오브젝트를 찾아주는 함수

        Transform fireRoot = transform.GetChild(0);  // 이 게임 오브젝트의 첫번째 자식 찾기
        fireTransforms = new Transform[fireRoot.childCount];
        for(int i = 0; i < fireTransforms.Length; i++)
        {
            fireTransforms[i] = fireRoot.GetChild(i);
        }

        // transform.childCount; // 이 게임 오브젝트의 자식 숫자

        fireFlash = transform.GetChild(1).gameObject;   // 이 게임 오브젝트의 두번째 자식의 게임오브젝트 찾기
        flashWait = new WaitForSeconds(0.1f);

        fireCoroutine = FireCoroutine();

    }

    // 이 스크립트가 포함된 게임 오브젝트가 활성화되면 호출된다.
    private void OnEnable()
    {
        inputActions.Player.Enable();                       // 활성화될 때 Player액션맵을 활성화
        inputActions.Player.Fire.performed += OnFireStart;  // Player액션맵의 Fire액션에 OnFireStart함수를 연결(눌렀을 때만 연결된 함수 실행)
        inputActions.Player.Fire.canceled += OnFireEnd;     // Player액션맵의 Fire액션에 OnFireEnd함수를 연결(땠을 때만 연결된 함수 실행)
        inputActions.Player.Boost.performed += OnBoost;
        inputActions.Player.Boost.canceled += OnBoost;
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }

    // 이 스크립트가 포함된 게임 오브젝트가 비활성화되면 호출된다.
    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Boost.canceled -= OnBoost;
        inputActions.Player.Boost.performed -= OnBoost;
        inputActions.Player.Fire.canceled -= OnFireEnd;     // Player액션맵의 Fire액션에 OnFireEnd함수를 연결해제
        inputActions.Player.Fire.performed -= OnFireStart;  // Player액션맵의 Fire액션에서 OnFireStart함수를 연결해제
        inputActions.Player.Disable();                      // Player액션맵을 비활성화
    }
    
    // 이 스크립트가 포함된 게임 오브젝트의 첫번째 Update함수가 실행되기 직전에 호출된다.
    private void Start()
    {
        // 파워와 생명 초기화
        Power = 1;
        Life = StartLife;
    }

    //private void Update()
    //{
    //    // 인풋매니저 방식
    //    //if (Input.GetKeyDown(KeyCode.A))
    //    //{
    //    //    Debug.Log("A키가 눌러졌습니다.");
    //    //}

    //    //if (Input.GetKeyUp(KeyCode.A))
    //    //{
    //    //    Debug.Log("A키가 떨어졌습니다.");
    //    //}
    //    // 실습
    //    // 누르고 있으면 [계속] 그쪽 방향으로 이동하게 만들어보기
    //    //transform.position += inputDir;     // OnMove에서 입력된 방향으로 움직이기

    //    // Time.deltaTime : 프레임간의 시간 간격(가변적)
    //    //transform.Translate(Time.deltaTime * moveSpeed * inputDir); // 1초당 moveSpeed만큼의 속도로, inputDir 방향으로 움직여라
        
    //}

    /// <summary>
    /// 고정된 시간간격으로 호출되는 업데이트(물리연산 처리용 업데이트)
    /// </summary>
    private void FixedUpdate()
    {
        if( IsAlive )   // 살아있을 때만 이동하기
        {
            //transform.Translate(Time.deltaTime * moveSpeed * inputDir);
            rigid2d.MovePosition(rigid2d.position + (Vector2)(Time.fixedDeltaTime * moveSpeed * inputDir));
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌이 시작했을 때 실행
        //Debug.Log($"OnCollisionEnter2D : {collision.gameObject.name}");

        //if( collision.gameObject.CompareTag("Wave") )  // collision의 게임 오브젝트가 "Wave"라는 태그를 가지는지 확인하는 함수
        //{
        //    Destroy(collision.gameObject);  // 충돌한 대상을 제거하기
        //}
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Life--;
        }
        else if( collision.gameObject.CompareTag("PowerUp") )
        {
            Power++;
            collision.gameObject.SetActive(false);
        }
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    // 충돌 중인 상태에서 움직일 때 실행
    //    //Debug.Log("OnCollisionStay2D");
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // 충돌 상태에서 떨어졌을 때 실행
    //    //Debug.Log("OnCollisionExit2D");
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    // 겹치기 시작했을 때 실행
    //    //Debug.Log($"OnTriggerEnter2D : {collision.gameObject.name}");
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    // 겹쳐있는 상태에서 움직일 때 실행
    //    //Debug.Log("OnTriggerStay2D");
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    // 겹쳐있던 것이 떨어졌을 때 실행
    //    //Debug.Log("OnTriggerExit2D");
    //}


    /// <summary>
    /// Fire액션이 발동했을 때 실행 시킬 함수
    /// </summary>
    /// <param name="context">입력 관련 정보가 들어있는 구조체 변수</param>
    private void OnFireStart(InputAction.CallbackContext _)
    {
        //if(context.performed)   // 지금 입력이 눌렀다
        //{
        //Debug.Log("OnFireStart : 눌려짐");
        //Fire(fireTransform.position);
        //}
        //if(context.canceled)    // 지금 입력이 떨어졌다
        //{
        //    Debug.Log("OnFireStart : 떨어짐");
        //}        
        StartCoroutine(fireCoroutine);  // 연사시작
    }

    private void OnFireEnd(InputAction.CallbackContext _)
    {
        StopCoroutine(fireCoroutine);   // 연사 중지
    }

    /// <summary>
    /// 연사용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < Power; i++)
            {
                Fire(fireTransforms[i]);                   // 총알 한발 쏘기
            }
            yield return new WaitForSeconds(fireInterval);  // 인터벌만큼 기다리기
        }
    }

    /// <summary>
    /// 총알을 하나 발사하는 함수
    /// </summary>
    /// <param name="fireTransform">총알이 발사될 트랜스폼</param>
    void Fire(Transform fireTransform)
    {
        //플래시 켜고 끄기
        StartCoroutine(FlashEffect());

        //Instantiate(bulletPrefab, transform); // 발사된 총알도 플레이어의 움직임에 영향을 받는다.
        //Instantiate(bulletPrefab, position, Quaternion.identity);

        Factory.Instance.GetBullet(fireTransform.position, fireTransform.eulerAngles.z);   // 팩토리를 이용해 총알 생성
    }

    IEnumerator FlashEffect()
    {
        fireFlash.SetActive(true);  // fireFlash 활성화 하기

        yield return flashWait;     // 0.1초 기다리기

        fireFlash.SetActive(false); // fireFlash 비활성화하기
    }

    // 실습
    // Boost 액션과 OnBoost 함수 연결하기
    // Boost 액션으로 눌려졌는지 떨어졌는지 출력하기
    private void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)   // 지금 입력이 눌렀다
        {
            Debug.Log("OnBoost : 눌려짐");
        }
        if (context.canceled)    // 지금 입력이 떨어졌다
        {
            Debug.Log("OnBoost : 떨어짐");
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // scope : 변수나 함수의 사용 가능한 범위
        inputDir = context.ReadValue<Vector2>();
        //Debug.Log($"OnMove : ({direction})");

        //this.transform.position = new Vector3(1, 0, 0); // 이 게임 오브젝트의 위치를 (1,0,0)으로 보내라

        //transform.position += new Vector3(1, 0, 0);   // 이 게임 오브젝트의 위치를 현재 위치에서 (1,0,0)만큼 움직여라
        //transform.position += Vector3.right;

        //transform.position += (Vector3)direction;   // 이 게임 오브젝트의 위치를 현재 위치에서 inputDir 방향으로 움직여라


        //anim.SetFloat("InputY", inputDir.y);    // 애니메이터가 가지는 InputY 파라메터에 inputDir.y값을 넣기
        anim.SetFloat(InputY_String, inputDir.y);
    }

    /// <summary>
    /// 점수를 추가해주는 변수
    /// </summary>
    /// <param name="getScore">새로 얻은 점수</param>
    public void AddScore(int getScore)
    {
        Score += getScore;
    }

    /// <summary>
    /// 파워 단계에 따라 총알 발사 위치 조정
    /// </summary>
    private void RefreshFirePositions()
    {
        for (int i = 0; i < MaxPower; i++)
        {
            if (i < Power)   // 파워 단계에 맞게 사용되는 부분 조정
            {
                // 1: 0도
                // 2: +15도, -15도        => 30도 한번
                // 3: +30도, 0도, -30도   => 30도 두번

                float startAngle = (Power - 1) * (FireAngle * 0.5f);    // power에 따라 시작각도를 다르게 설정
                float angleDelta = i * -FireAngle;                      // 30도씩 단계별로 회전
                fireTransforms[i].rotation = Quaternion.Euler(0, 0, startAngle + angleDelta);

                fireTransforms[i].localPosition = Vector3.zero;         // 초기화
                fireTransforms[i].Translate(0.5f, 0.0f, 0.0f);          // 살짝 오른쪽으로 옮기기(로컬 기준)

                fireTransforms[i].gameObject.SetActive(true);           // 활성화
            }
            else
            {
                fireTransforms[i].gameObject.SetActive(false);          // 비활성화
            }
        }
    }

    /// <summary>
    /// 맞았을 때 실행되는 함수
    /// </summary>
    void OnHit()
    {
        //Debug.Log($"플레이어의 생명이 {life}남았다.");

        Power--;    // 파워 1감소
        StartCoroutine(InvinvibleMode());   // 무적모드에 들어감
    }

    /// <summary>
    /// 무적 모드 처리용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator InvinvibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible"); // 레이어를 무적 레이어로 변경

        float timeElapsed = 0.0f;
        while (timeElapsed < invincibleTime) // 2초동안 계속하기
        {
            timeElapsed += Time.deltaTime;

            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.5f;   // 코사인 결과를 1 ~ 0 사이로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);               // 알파에 지정(깜박거리게 된다.)

            yield return null;
        }

        // 2초가 지난후
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어를 다시 플레이어로 되돌리기
        spriteRenderer.color = Color.white;                 // 알파값도 원상복구

    }

    /// <summary>
    /// 플레이어 사망 처리용 함수
    /// </summary>
    void OnDie()
    {
        Debug.Log("플레이어가 죽었다.");

        // 충돌이 안 일어나야함
        Collider2D body = GetComponent<Collider2D>();
        body.enabled = false;       // 이 컴포넌트만 끄기. 더 이상 충돌이 안일어나게 만들기        

        // 내가 터지는 이팩트
        Factory.Instance.GetExplosionEffect(transform.position);

        // 입력도 안되어야 함
        inputActions.Player.Disable();

        // 이동 초기화, 총알 발사 코루틴 정지 => disable하면서 자동으로 입력이 디폴트로 돌아가서 안해도 된다.

        // 튕겨나가는 듯한 연출을 보여줘야 한다.
        rigid2d.gravityScale = 1.0f;
        rigid2d.freezeRotation = false;
        rigid2d.AddTorque(10000);
        rigid2d.AddForce(Vector2.left * 10.0f, ForceMode2D.Impulse);

        // 사망했음을 알림
        onDie?.Invoke(Score);
    }


#if UNITY_EDITOR    // unity 에디터에서 실행한 경우만 true
    public void Test_PowerUp()
    {
        Power++;
    }

    public void Test_PowerDown()
    {
        Power--;
    }

    public void Test_Die()
    {
        Life = 0;
    }

    public void Test_SetScore(int score)
    {
        Score = score;
    }
#endif
}
