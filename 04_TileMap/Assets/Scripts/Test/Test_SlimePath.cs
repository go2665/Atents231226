using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_SlimePath : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Slime slime;

    TileGridMap map;

    private void Start()
    {
        map = new TileGridMap(background, obstacle);
        slime.Initialize(map, new(1,1));
    }

    protected override void OnTestLClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if(map.IsValidPosition(gridPos) && map.IsPlain(gridPos))
        {
            slime.SetDestination(gridPos);
        }
    }
}

/*
디버그용 단축키
 - F5 : 디버그 모드 시작, 다음 브레이크 포인트까지 진행
 - F9 : 브레이크 포인트 잡기
 - F10 : 다음 라인으로 넘어가기
 - F11: 지금 진행중인 라인에 있는 함수 내부로 들어가기

호출스택(Call Stack)
 - 함수가 어떤 어떤 함수에 의해 호출이 되었는지를 알 수 있다.

조사식
 - 변수를 등록해서 값을 확인할 수 있다.
 */


// 실습
// 1. 슬라임이 목적지에 도착하면 새로운 목적지를 랜덤으로 설정한다.(GetRandomMoveablePosition함수 사용)
// 2. 페이즈나 디졸브 도중에 움직이지 않아야 한다.