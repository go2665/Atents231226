using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Cell : MonoBehaviour
{    
    /// <summary>
    /// 이 셀의 ID(위치계산에도 사용될 수 있음)
    /// </summary>
    int? id = null;

    /// <summary>
    /// ID 설정 및 확인용 프로퍼티
    /// </summary>
    public int ID
    {
        get => id.GetValueOrDefault();  // 0일 경우 맞을 수도 있고 아닐수도 있다.
        set
        {
            if( id == null )    // 이 프로퍼티는 한번만 설정 가능하다.
            {
                id = value;
            }
        }
    }

    /// <summary>
    /// 겉면의 스프라이트 랜더러(Close, Question, Flag)
    /// </summary>
    SpriteRenderer cover;

    /// <summary>
    /// 안쪽의 스프라이트 랜더러(지뢰, 주변 지뢰 개수)
    /// </summary>
    SpriteRenderer inside;

    /// <summary>
    /// 셀에 지뢰가 있는지 여부
    /// </summary>
    bool hasMine = false;

    /// <summary>
    /// 지뢰 설치 여부를 확인하기 위한 프로퍼티
    /// </summary>
    public bool HasMine => hasMine;

    /// <summary>
    /// 이 셀이 관리되는 보드
    /// </summary>
    Board parentBoard = null;
    public Board Board
    {
        get => parentBoard;
        set
        {
            if(parentBoard == null )    // 한번만 설정 가능
            {
                parentBoard = value;
            }
        }
    }

    /// <summary>
    /// 자기 주변 셀의 목록
    /// </summary>
    List<Cell> neighbors;

    /// <summary>
    /// 자기 주변의 지뢰 개수
    /// </summary>
    int aroundMineCount = 0;

    /// <summary>
    /// 셀이 열렸는지 여부
    /// </summary>
    bool isOpen = false;

    /// <summary>
    /// 셀의 커버 표시 상태용(닫혔을 때의 상태)
    /// </summary>
    enum CellCoverState
    {
        None = 0,   // 아무것도 표시되지 않은 상태
        Flag,       // 깃발이 표시된 상태
        Question    // 물음표가 표시된 상태
    }

    /// <summary>
    /// 셀의 커버 상태
    /// </summary>
    CellCoverState coverState = CellCoverState.None;

    /// <summary>
    /// 셀의 커버 상태를 설정하고 확인하기 위한 프로퍼티
    /// </summary>
    CellCoverState CoverState
    {
        get => coverState;
        set
        {
            coverState = value;
            switch (coverState)
            {
                case CellCoverState.None:
                    cover.sprite = Board[CloseCellType.Close];
                    break;
                case CellCoverState.Flag:
                    cover.sprite = Board[CloseCellType.Flag];
                    onFlagUse?.Invoke();        // 깃발이 설치되었음을 알림
                    break;
                case CellCoverState.Question:
                    cover.sprite = Board[CloseCellType.Question];
                    onFlagReturn?.Invoke();     // 깃발이 해제되었음을 알림
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 깃발 설치 여부를 알려주는 프로퍼티
    /// </summary>
    public bool IsFlaged => CoverState == CellCoverState.Flag;

    /// <summary>
    /// 이 셀에 의해 눌려진 셀의 목록(자기자신 or 자기주변에 닫혀 있던 셀들)
    /// </summary>
    List<Cell> pressedCells;

    /// <summary>
    /// 깃발이 설치 되었음을 알리는 델리게이트
    /// </summary>
    public Action onFlagUse;

    /// <summary>
    /// 깃발 설치가 취소 되었음을 알리는 델리게이트
    /// </summary>
    public Action onFlagReturn;

    /// <summary>
    /// 지뢰가 터졌음을 알리는 델리게이트
    /// </summary>
    public Action onExplosion;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        cover = child.GetComponent<SpriteRenderer>();
        child = transform.GetChild(1);
        inside = child.GetComponent<SpriteRenderer>();

        pressedCells = new List<Cell>(8);
    }

    /// <summary>
    /// 셀 생성 초기화 함수(처음에 단 한번만 실행되면 됨)
    /// </summary>
    public void Initialize()
    {
        neighbors = Board.GetNeightbors(ID);    // 이웃 셀 저장해 놓기
    }

    /// <summary>
    /// 이 셀의 데이터를 초기화 하는 함수
    /// </summary>
    public void ResetData()
    {
        hasMine = false;
        aroundMineCount = 0;
        isOpen = false;

        pressedCells.Clear();

        CoverState = CellCoverState.None;
        cover.sprite = Board[CloseCellType.Close];
        inside.sprite = Board[OpenCellType.Empty];
        cover.gameObject.SetActive(true);
    }

    /// <summary>
    /// 이 셀에 지뢰를 설치하는 함수
    /// </summary>
    public void SetMine()
    {
        hasMine = true;                             // 지뢰 설치했다고 표시
        inside.sprite = Board[OpenCellType.Mine];   // 이미지 변경

        foreach(Cell cell in neighbors)
        {
            cell.IncreaseAroundMineCount(); // 주변 셀의 주변 지뢰 개수 증가
        }
    }

    /// <summary>
    /// 주변 지뢰 개수 증가용 함수
    /// </summary>
    void IncreaseAroundMineCount()
    {
        if (!hasMine)   // 지뢰가 아닐 때만 개수 증가
        {
            aroundMineCount++;                                      // 주변 지뢰 개수 증가
            inside.sprite = Board[(OpenCellType)aroundMineCount];   // 주변 지뢰 개수에 맞게 이미지 변경
        }
    }

    /// <summary>
    /// 셀이 우클릭되면 실행되는 함수
    /// </summary>
    public void RightPress()
    {
        switch (CoverState)
        {
            case CellCoverState.None:
                CoverState = CellCoverState.Flag;
                break;
            case CellCoverState.Flag:
                CoverState = CellCoverState.Question;
                break;
            case CellCoverState.Question:
                CoverState = CellCoverState.None;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 셀이 마우스 왼쪽버튼으로 눌려졌을 때 실행되는 함수
    /// (셀에 누르는 표시를 한다.)
    /// </summary>
    public void LeftPress()
    {
        pressedCells.Clear();   // 눌려진 셀들 초기화
        
        if (isOpen)
        {
            // 열려있는 셀을 눌렀을 때
            foreach(var cell in neighbors)          // 모든 이웃 셀에 대해
            {
                if(!cell.isOpen && !cell.IsFlaged)  // 닫혀있고 깃발표시가 안되어 있는 셀만
                {
                    pressedCells.Add(cell);         // 눌려진 셀로 기록
                    cell.LeftPress();               // 누르는 처리 실행
                }
            }
        }
        else
        {
            // 닫혀있는 셀을 눌렀을 때
            switch (CoverState)         // 커버 상태에 따라 눌려진 이미지로 변경
            {
                case CellCoverState.None:
                    cover.sprite = Board[CloseCellType.ClosePress];
                    break;
                case CellCoverState.Question:
                    cover.sprite = Board[CloseCellType.QuestionPress];
                    break;
                //case CellCoverState.Flag:
                default:
                    // 하는 일 없음
                    break;
            }
            pressedCells.Add(this);     // 눌려진 셀로 기록
        }        
    }

    /// <summary>
    /// 셀이 마우스 왼쪽버튼으로 누르고 있다가 땠을 때 실행되는 함수
    /// (눌려진 상태 복구용)
    /// </summary>
    public void LeftRelease()
    {
        if(isOpen)
        {
            // 열려 있는 셀의 경우(주변셀 누르기)
            int flagCount = 0;
            foreach(var cell in neighbors)          // 주변 깃발 개수 카운팅
            {
                if(cell.IsFlaged)
                    flagCount++;
            }

            if(aroundMineCount == flagCount)        // 주변 지뢰 개수와 깃발 개수가 같으면
            {
                foreach(var cell in pressedCells)
                {
                    cell.Open();    // 눌려진 셀을 전부 연다.
                }
            }
            else
            {
                RestoreCovers();    // 주변 지뢰 개수와 깃발 개수가 다르면 눌려진 셀을 전부 복구
            }
        }
        else
        {
            // 닫혀 있는 셀의 경우
            Open();     // 그냥 열기
        }
    }

    /// <summary>
    /// 셀을 여는 함수
    /// </summary>
    void Open()
    {
        if(!isOpen && !IsFlaged)    // 닫혀있고 깃발이 설치되어 있지 않은 셀만 처리
        {
            isOpen = true;                      // 열렸다고 표시
            cover.gameObject.SetActive(false);  // 커버 제거

            if (hasMine)                        // 지뢰가 있다.
            {
                //Debug.Log("게임 오버");
                inside.sprite = Board[OpenCellType.Mine_Explotion]; // 터지는 이미지로 변경
                onExplosion?.Invoke();          // 지뢰가 터짐을 알림
            }
            else if (aroundMineCount <= 0)      // 지뢰가 없고 주변 지뢰개수가 0이하다.(비어있는 셀)
            {
                foreach( var cell in neighbors )    // 주변 이웃 모두 열기
                {
                    cell.Open();
                }
            }
        }
    }

    /// <summary>
    /// 원래 커버 이미지로 변경하는 함수
    /// </summary>
    void RestoreCover()
    {
        switch (CoverState)
        {
            case CellCoverState.None:
                cover.sprite = Board[CloseCellType.Close];
                break;
            case CellCoverState.Question:
                cover.sprite = Board[CloseCellType.Question];
                break;
            //case CellCoverState.Flag:
            default:
                // 하는 일 없음
                break;
        }
    }

    /// <summary>
    /// 눌려진 셀을 모두 원래 커버 이미지로 변경하는 함수
    /// </summary>
    public void RestoreCovers()
    {
        foreach (var cell in pressedCells)
        {
            cell.RestoreCover();
        }
        pressedCells.Clear();
    }

    /// <summary>
    /// 지뢰가 아닌데 지뢰로 잘못 설정했을 때 표시용
    /// </summary>
    public void FlagMistake()
    {
        cover.gameObject.SetActive(false);
        inside.sprite = Board[OpenCellType.Mine_Mistake];
    }

    /// <summary>
    /// 지뢰인데 못찾았을 때 표시용
    /// </summary>
    public void MineNotFound()
    {
        cover.gameObject.SetActive(false);
    }


#if UNITY_EDITOR
    public void Test_OpenCover()
    {
        cover.gameObject.SetActive(false);
    }
#endif

}
