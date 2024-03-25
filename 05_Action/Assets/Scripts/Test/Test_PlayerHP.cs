using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerHP : TestBase
{
    Player player;
    public float data = 10.0f;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        // 플레이어 HP 증가
        player.HP += data;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // 플레이어 HP 감소
        player.HP -= data;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // 플레이어 HP 재생
        player.HealthRegenerate(data, 1);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // 플레이어 HP 틱당 재생
        player.HealthRegenerateByTick(3, 0.5f, 4);
    }
}

// 실습
// 1. IMana 인터페이스를 만들고 플레이어에게 상속 시키기
// 2. IHealth, IMana를 사용하는 플레이어용 UI만들기(PlayerInfo)
//  2.1. 화면 왼쪽 위에 배치
//  2.2. 플레이어의 HP와 MP가 변경되면 UI도 같이 변해야 한다.
//  2.3. 숫자로 현재 수치와 최대 수치 표현되어야 함
//  2.4. 두개의 게이지는 색상이 달라야 한다.