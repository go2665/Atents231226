using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public Color userColor;
    public Color enemyColor;
    public Color shipColor;
    public Color turnColor;

    // 실습
    // 1. public 함수 없음(테스트는 예외)
    // 2. 턴 시작시에 몇번째 턴이 시작되었는지 출력하기(턴 번호는 다른 색으로 출력하기)
    // 3. 공격시에 공격 성공/실패 출력하기(공격 실패 델리게이트 필요)
    //  3.1. 성공 예시) : "[적]의 공격 : [당신]의 [항공모함]에 포탄이 명중했습니다."
    //  3.2. 실패 예시) : "[나]의 공격 : [나]의 포탄이 빗나갔습니다."
    // 4. 함선이 침몰하면 침몰했다고 출력하기
    //  4.1. 예시) "[나]의 공격 : [적]의 [구축함]이 침몰했습니다."
    // 5. 게임이 종료되면 상황 출력하기
    //  5.1. 예시) "[당신]의 승리!"
    //  5.2. 예시) "[당신]의 패배..."

}
