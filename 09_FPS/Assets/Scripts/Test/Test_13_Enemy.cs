using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_13_Enemy : TestBase
{
    public Enemy enemy;
    public Transform respawn;

    private void Start()
    {
        enemy.Respawn(respawn.position);
    }

}

// 적의 상태 구현하기
// 1. Wander : 랜덤으로 계속 이동(자기 현재 위치에서 일정 반경 안을 랜덤하게 계속 이동하기)

//public enum BehaviorState : byte
//{
//    Wander = 0, // 배회상태. 주변을 왔다갔다한다.
//    Chase,      // 추적상태. 플레이어가 마지막으로 목격된 장소를 향해 계속 이동한다.
//    Find,       // 탐색상태. 추적 도중에 플레이어가 시야에서 사라지면 두리번 거리며 주변을 찾는다.
//    Attack,     // 공격상태. 추적 상태일 때 플레이어가 일정범위안에 들어오면 일정 주기로 공격을 한다.
//    Dead        // 사망상태. 죽는다.(일정 시간 후에 재생성)
//}

// 기즈모 그리기
