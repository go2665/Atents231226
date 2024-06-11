using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_13_Enemy : TestBase
{
    public Enemy enemy;
    public Transform respawn;

    public Enemy.BehaviorState behaviorState = Enemy.BehaviorState.Wander;

    private void Start()
    {
        enemy.Respawn(respawn.position);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Vector3 pos = enemy.Test_GetRandomPosition();
        Debug.Log(pos);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        enemy.Test_StateChange(behaviorState);

        // OnStateEnter에서 눈 색깔 변경하기
        // 배회 : 녹색
        // 추적 : 주황색
        // 탐색 : 파랑색
        // 공격 : 빨강색
        // 사망 : 검정색
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        enemy.Test_EnemyStop();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Factory.Instance.GetDropItem(Enemy.ItemTable.Shotgun, respawn.position);
    }

}

// 적의 상태 구현하기
// 1. Wander : 랜덤으로 계속 이동(자기 현재 위치에서 일정 반경 안을 랜덤하게 계속 이동하기)
// 2. Chase : 플레이어가 마지막으로 목격된 장소를 향해 계속 이동하기(IsPlayerInSight 구현하기)
// 3. Find : 플레이어가 시야범위에서 벗어나면 몇초동안 두리번거리기(시간이 다되면 Wander 상태로 변경, 플레이어를 찾으면 다시 Chase로 변경)

//public enum BehaviorState : byte
//{
//    Wander = 0, // 배회상태. 주변을 왔다갔다한다.
//    Chase,      // 추적상태. 플레이어가 마지막으로 목격된 장소를 향해 계속 이동한다.
//    Find,       // 탐색상태. 추적 도중에 플레이어가 시야에서 사라지면 두리번 거리며 주변을 찾는다.
//    Attack,     // 공격상태. 추적 상태일 때 플레이어가 일정범위안에 들어오면 일정 주기로 공격을 한다.
//    Dead        // 사망상태. 죽는다.(일정 시간 후에 재생성)
//}

// 기즈모 그리기
