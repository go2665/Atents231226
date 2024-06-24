using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float moveSpeed = 5.0f;

    Vector3 forward = Vector3.forward;

    NetworkCharacterController cc;

    [SerializeField] 
    Ball prefabBall;

    [Networked]
    TickTimer delay { get; set; }

    private void Awake()
    {
        cc = GetComponent<NetworkCharacterController>();
    }

    /// <summary>
    /// 네트워크 틱별로 계속 실행되는 함수
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))     // 서버쪽에서 입력 정보 받아오기
        {
            //data.direction.Normalize();             // 유닛벡터로 만들기

            cc.Move(Runner.DeltaTime * moveSpeed * data.direction); // 초당 moveSpeed의 속도로 data.direction방향으로 이동

            if(data.direction.sqrMagnitude > 0)
            {
                forward = data.direction;           // 회전 도중에 forward방향으로 공이 발사되는 것을 방지
            }

            if(HasStateAuthority && delay.ExpiredOrNotRunning(Runner))      // 호스트인지 확인 && delay가 설정 안되었거나 0.5초 설정하고 만료되었는지 확인
            {
                if(data.buttons.IsSet(NetworkInputData.MouseButtonLeft))    // 마우스 왼쪽 버튼이 눌려져 있다.
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);      // 발사 쿨타임 0.5초 지정
                    Runner.Spawn(
                        prefabBall,                         // 생성할 프리팹
                        transform.position + forward,       // 생성될 위치(자기 위치 + 입력방향)
                        Quaternion.LookRotation(forward),   // 생성될 회전(입력방향쪽으로)
                        Object.InputAuthority,              // 생성한 플레이어의 입력 권한?(생성한 플레이어?)
                        (runner, obj) =>                    // 스폰 직전에 실행되는 람다식
                        {
                            obj.GetComponent<Ball>().Init();
                        });
                }
            }

        }
    }
}
