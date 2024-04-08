using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Press : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.Board.Test_BoardReset();
    }
}

// 1. 열려 있는 셀을 누르면 열려있는 셀의 주변 모든 셀이 눌려진다.(깃발 표시된 셀은 제외)
// 2. 열려 있는 셀에서 마우스를 땠을 때
//      열려있던 겔의 aroundMineCount와 눌려진 셀에 표시된 깃발 개수가 같으면
//          깃발 표시가 되지 않은 나머지 셀을 모두 연다.
//      열려있던 겔의 aroundMineCount와 눌려진 셀에 표시된 깃발 개수가 다르면
//          전부 복구