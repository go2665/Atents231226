using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    protected Board board;
    public Board Board => board;

    protected Ship[] ships;
    public Ship[] Ships => ships;

    protected virtual void Awake()
    {
        Transform child = transform.GetChild(0);
        board = child.GetComponent<Board>();
    }

    protected virtual void Start()
    {
        Initialize();
    }

    protected void Initialize()
    {

    }

    // 함선배치 관련 함수 --------------------------------------------------------------------------

    // 공격 관련 함수 ------------------------------------------------------------------------------
    
    // 턴 관리용 함수 ------------------------------------------------------------------------------
    // 함선 침몰 빛 패배처리 함수 -------------------------------------------------------------------
    // 기타 ---------------------------------------------------------------------------------------
    // 테스트 --------------------------------------------------------------------------------------
}
