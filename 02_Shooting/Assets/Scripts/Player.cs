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

    /// <summary>
    /// 총알의 프리팹
    /// </summary>
    public GameObject bulletPrefab;

    Transform fireTransform;

    // 이 스크립트가 포함된 게임 오브젝트가 생성 완료되면 호출된다.
    private void Awake()
    {
        inputActions = new PlayerInputActions();            // 인풋 액션 생성
        anim = GetComponent<Animator>();   // 이 스크립트가 들어있는 게임 오브젝트에서 컴포넌트를 찾아서 anim에 저장하기(없으면 null)
        // null; // 참조가 비어있다.
        rigid2d = GetComponent<Rigidbody2D>();

        // 게임 오브젝트 찾는 방법
        // GameObject.Find("FirePosition"); // 이름으로 게임 오브젝트 찾기
        // GameObject.FindAnyObjectByType<Transform>(); // 특정 컴포넌트를 가지고 있는 게임 오브젝트 찾기
        // GameObject.FindFirstObjectByType<Transform>();  // 특정 컴포넌트를 가지고 있는 첫번째 게임 오브젝트 찾기
        // GameObject.FindGameObjectWithTag("Player");  // 게임 오브젝트의 태그를 기준으로 찾는 함수
        // GameObject.FindGameObjectsWithTag("Player"); // 특정 태그를 가진 모든 게임오브젝트를 찾아주는 함수

        fireTransform = transform.GetChild(0);  // 이 게임 오브젝트의 첫번째 자식 찾기
        // transform.childCount; // 이 게임 오브젝트의 자식 숫자
    }

    // 이 스크립트가 포함된 게임 오브젝트가 활성화되면 호출된다.
    private void OnEnable()
    {
        inputActions.Player.Enable();                       // 활성화될 때 Player액션맵을 활성화
        inputActions.Player.Fire.performed += OnFire;       // Player액션맵의 Fire액션에 OnFire함수를 연결(눌렀을 때만 연결된 함수 실행)
        inputActions.Player.Fire.canceled += OnFire;        // Player액션맵의 Fire액션에 OnFire함수를 연결(땠을 때만 연결된 함수 실행)
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
        inputActions.Player.Fire.canceled -= OnFire;        // Player액션맵의 Fire액션에 OnFire함수를 연결해제
        inputActions.Player.Fire.performed -= OnFire;       // Player액션맵의 Fire액션에서 OnFire함수를 연결해제
        inputActions.Player.Disable();                      // Player액션맵을 비활성화
    }

    /// <summary>
    /// Fire액션이 발동했을 때 실행 시킬 함수
    /// </summary>
    /// <param name="context">입력 관련 정보가 들어있는 구조체 변수</param>
    private void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)   // 지금 입력이 눌렀다
        {
            //Debug.Log("OnFire : 눌려짐");
            //Instantiate(bulletPrefab, transform); // 발사된 총알도 플레이어의 움직임에 영향을 받는다.
            Instantiate(bulletPrefab, fireTransform.position, Quaternion.identity);
        }
        //if(context.canceled)    // 지금 입력이 떨어졌다
        //{
        //    Debug.Log("OnFire : 떨어짐");
        //}        
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
        //Debug.Log($"OnMove : ({dir})");

        //this.transform.position = new Vector3(1, 0, 0); // 이 게임 오브젝트의 위치를 (1,0,0)으로 보내라

        //transform.position += new Vector3(1, 0, 0);   // 이 게임 오브젝트의 위치를 현재 위치에서 (1,0,0)만큼 움직여라
        //transform.position += Vector3.right;

        //transform.position += (Vector3)dir;   // 이 게임 오브젝트의 위치를 현재 위치에서 inputDir 방향으로 움직여라


        //anim.SetFloat("InputY", inputDir.y);    // 애니메이터가 가지는 InputY 파라메터에 inputDir.y값을 넣기
        anim.SetFloat(InputY_String, inputDir.y);
    }
    
    // 이 스크립트가 포함된 게임 오브젝트의 첫번째 Update함수가 실행되기 직전에 호출된다.
    private void Start()
    {
        
    }

    private void Update()
    {
        // 인풋매니저 방식
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log("A키가 눌러졌습니다.");
        //}

        //if (Input.GetKeyUp(KeyCode.A))
        //{
        //    Debug.Log("A키가 떨어졌습니다.");
        //}
        // 실습
        // 누르고 있으면 [계속] 그쪽 방향으로 이동하게 만들어보기
        //transform.position += inputDir;     // OnMove에서 입력된 방향으로 움직이기

        // Time.deltaTime : 프레임간의 시간 간격(가변적)
        //transform.Translate(Time.deltaTime * moveSpeed * inputDir); // 1초당 moveSpeed만큼의 속도로, inputDir 방향으로 움직여라
        
    }

    /// <summary>
    /// 고정된 시간간격으로 호출되는 업데이트(물리연산 처리용 업데이트)
    /// </summary>
    private void FixedUpdate()
    {
        //transform.Translate(Time.deltaTime * moveSpeed * inputDir);
        rigid2d.MovePosition(rigid2d.position + (Vector2)(Time.fixedDeltaTime * moveSpeed * inputDir));

    }

    //public void OnFire()
    //{
    //    Debug.Log("OnFire");
    //}
}
