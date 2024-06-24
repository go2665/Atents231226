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

    [SerializeField]
    PhysxBall prefabPhysxBall;

    [Networked]
    TickTimer delay { get; set; }

    [Networked]
    public bool spawnedProjectile { get; set; }

    /// <summary>
    /// Networked로 설정된 변수의 변화를 감지하는 클래스
    /// </summary>
    private ChangeDetector changeDetector;

    /// <summary>
    /// 플레이어 몸통 머티리얼
    /// </summary>
    public Material bodyMaterial;

    private void Awake()
    {
        cc = GetComponent<NetworkCharacterController>();
        Transform child = transform.GetChild(0);
        bodyMaterial = child.GetComponent<Renderer>()?.material;
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
                    spawnedProjectile = !spawnedProjectile;
                }
                if(data.buttons.IsSet(NetworkInputData.MouseButtonRight))
                {
                    delay = TickTimer.CreateFromSeconds(Runner, 0.5f);      // 발사 쿨타임 0.5초 지정
                    Runner.Spawn(
                        prefabPhysxBall,                    // 생성할 프리팹
                        transform.position + forward + Vector3.up * 0.5f,   // 생성될 위치(자기 위치 + 입력방향)
                        Quaternion.LookRotation(forward),   // 생성될 회전(입력방향쪽으로)
                        Object.InputAuthority,              // 생성한 플레이어의 입력 권한?(생성한 플레이어?)
                        (runner, obj) =>                    // 스폰 직전에 실행되는 람다식
                        {
                            obj.GetComponent<PhysxBall>().Init(moveSpeed * forward);
                        });
                    spawnedProjectile = !spawnedProjectile;
                }
            }
        }
    }

    /// <summary>
    /// 스폰 된 이후에 실행되는 함수
    /// </summary>
    public override void Spawned()
    {
        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        // 이 네트워크 오브젝트에서 Networked로 설정된 변수에 변화가 있었던 것들을 모두 순회
        foreach (string change in changeDetector.DetectChanges(this))    
        {
            switch(change)
            {
                case nameof(spawnedProjectile): // spawnedProjectile 변수가 변경되었을 때 
                    bodyMaterial.color = Color.white;
                    break;

            }
        }

        // Render는 유니티 랜더 루프상에서 작동 => Update와 같은 간격
        bodyMaterial.color = Color.Lerp(bodyMaterial.color, Color.blue, Time.deltaTime * 2);
    }
}
