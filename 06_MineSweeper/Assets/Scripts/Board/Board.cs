using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    /// <summary>
    /// 생성할 셀의 프리팹
    /// </summary>
    public GameObject cellPrefab;

    /// <summary>
    /// 보드의 가로 길이
    /// </summary>
    int width = 16;

    /// <summary>
    /// 보드의 세로 길이
    /// </summary>
    int height = 16;

    /// <summary>
    /// 배치될 지뢰의 개수
    /// </summary>
    int mineCount = 10;

    /// <summary>
    /// 이 보드가 생성한 모든 셀
    /// </summary>
    Cell[] cells;

    /// <summary>
    /// 셀 한 변의 크기
    /// </summary>
    const float Distance = 1.0f;

    /// <summary>
    /// 현재 마우스가 누르고 있는 셀
    /// </summary>
    Cell currentCell = null;

    /// <summary>
    /// 현재 마우스가 누르고 있는 셀을 확인하고 설정하는 프로퍼티
    /// </summary>
    Cell CurrentCell
    {
        get => currentCell;
        set
        {
            if(currentCell != value)            // currentCell이 변경되면
            {
                currentCell?.RestoreCovers();   // 이전 currentCell이 눌려놓았던 것을 모두 원래대로 복구
                currentCell = value;
                currentCell?.LeftPress();       // 새 currentCell에 누르기 처리
            }
        }
    }

    public Action onBoardLeftPress;
    public Action onBoardLeftRelease;

    /// <summary>
    /// 인풋시스템을 위한 인풋액션
    /// </summary>
    PlayerInputActions inputActions;

    public Sprite[] openCellImage;
    public Sprite this[OpenCellType type] => openCellImage[(int)type];
    public Sprite[] closeCellImage;
    public Sprite this[CloseCellType type] => closeCellImage[(int)type];

    /// <summary>
    /// 게임 매니저
    /// </summary>
    GameManager gameManager;

    /// <summary>
    /// 현재 플레이 중인지 확인하는 프로퍼티
    /// </summary>
    public bool IsPlaying => gameManager.IsPlaying;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.LeftClick.performed += OnLeftPress;     // 왼버튼 누를 때
        inputActions.Player.LeftClick.canceled += OnLeftRelease;    // 왼버튼 땠을 때
        inputActions.Player.RightClick.performed += OnRightClick;   // 오른버튼 클릭
        inputActions.Player.MouseMove.performed += OnMouseMove;     // 마우스 움직임
    }

    private void OnDisable()
    {
        inputActions.Player.MouseMove.performed -= OnMouseMove;
        inputActions.Player.RightClick.performed -= OnRightClick;
        inputActions.Player.LeftClick.canceled -= OnLeftRelease;
        inputActions.Player.LeftClick.performed -= OnLeftPress;
        inputActions.Player.Disable();
    }

    /// <summary>
    /// 이 보드가 가질 모든 셀을 생성하고 배치하는 함수
    /// </summary>
    /// <param name="newWidth">보드의 가로 길이</param>
    /// <param name="newHieght">보드의 세로 길이</param>
    /// <param name="newMineCount">배치될 지뢰의 개수</param>
    public void Initialize(int newWidth, int newHieght, int newMineCount)
    {
        // 게임 매니저 저장해 놓기
        gameManager = GameManager.Instance;

        // 값 설정
        width = newWidth;
        height = newHieght;
        mineCount = newMineCount;

        // 셀 배열 만들기
        cells = new Cell[width * height];

        // 셀 하나씩 생성 후 배열에 추가
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject cellObj = Instantiate(cellPrefab, transform);    // 셀 게임 오브젝트 생성
                Cell cell = cellObj.GetComponent<Cell>();

                int id = x + y * width;
                cell.ID = id;               // ID 설정
                cell.transform.localPosition = new Vector3(x * Distance, -y * Distance);    // 위치 설정
                cell.Board = this;          // Board 기록해 두기

                cell.onFlagUse += gameManager.DecreaseFlagCount;        // 셀에 깃발 설치됬을 때 실행될 함수 연결
                cell.onFlagReturn += gameManager.IncreaseFlagCount;     // 셀의 깃발이 제거되었을 떄 실행될 함수 연결
                cell.onExplosion += gameManager.GameOver;               // 셀에서 지뢰가 터졌을 때 실행될 함수 연결

                cellObj.name = $"Cell_{id}_({x},{y})";      // 게임 오브젝트의 이름을 알아보기 쉽게 변경

                cells[id] = cell;   // 셀을 배열에 저장
            }
        }

        // 셀 전체 초기화
        foreach (Cell cell in cells)
        {
            cell.Initialize();
        }

        gameManager.onGameReady += ResetBoard;  // 레디로 가면 보드 리셋
        gameManager.onGameOver += OnGameOver;

        // 보드 데이터 리셋
        ResetBoard();
    }

    /// <summary>
    /// 보드에 존재하는 모들 셀의 데이터를 리셋하고 지뢰를 새로 배치하는 함수(게임 재시작용 함수)
    /// </summary>
    void ResetBoard()
    {
        // 전체 셀의 데이터 리셋
        foreach (Cell cell in cells)    
        {
            cell.ResetData();
        }

        // mineCount만큼 지뢰 배치하기
        Shuffle(cells.Length, out int[] shuffleResult); // 숫자 섞기(0 ~ cells.length-1)
        for(int i=0;i<mineCount;i++)
        {
            cells[shuffleResult[i]].SetMine();
        }
    }

    // 게임 메니저 상태 변화시 사용할 함수들 ----------------------------------------------------------------

    /// <summary>
    /// 게임오버가 되면 보드가 처리할 일을 기록해 놓은 함수
    /// </summary>
    private void OnGameOver()
    {
        // Debug.Log("보드 : 게임오버 신호를 받음");
        foreach (Cell cell in cells)
        {
            if(cell.IsFlaged && !cell.HasMine)
            {
                // 잘못 설치한 깃발은 Cell_Open_NotMine 스프라이트로 변경
                cell.FlagMistake();
            }
            else if(!cell.IsFlaged && cell.HasMine)
            {
                // 못찾은 지뢰는 커버를 제거해서 위치를 보여준다.
                cell.MineNotFound();
            }
        }
    }

    // 셀 확인용 함수들 ------------------------------------------------------------------------------------

    /// <summary>
    /// 스크린 좌표를 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="screen">스크린 좌표</param>
    /// <returns>변환된 그리드 좌표</returns>
    Vector2Int ScreenToGrid(Vector2 screen)
    {
        Vector2 world = Camera.main.ScreenToWorldPoint(screen);
        Vector2 diff = world - (Vector2)transform.position;

        return new Vector2Int(Mathf.FloorToInt(diff.x / Distance), Mathf.FloorToInt(-diff.y/Distance));
    }

    /// <summary>
    /// 그리드 좌표를 인덱스로 변경해주는 함수
    /// </summary>
    /// <param name="x">x위치</param>
    /// <param name="y">y위치</param>
    /// <returns>변환된 인덱스 값(잘못된 그리드 좌표면 null)</returns>
    int? GridToIndex(int x,  int y)
    {
        int? result = null;
        if(IsValidGrid(x, y))
        {
            result = x + y * height;
        }

        return result;
    }

    /// <summary>
    /// 인덱스 값을 그리드 좌표로 변경해주는 함수
    /// </summary>
    /// <param name="index">변경할 인덱스</param>
    /// <returns>변경 완료된 그리드 좌표</returns>
    Vector2Int IndexToGrid(int index)
    {
        return new(index % width, index / width);
    }

    /// <summary>
    /// 지정된 그리드 좌표가 보드 내부인지 확인하는 함수
    /// </summary>
    /// <param name="x">x좌표</param>
    /// <param name="y">y좌표</param>
    /// <returns>보드 안이면 true, 밖이면 false, 셀이 초기화 되지 않아도 false</returns>
    bool IsValidGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height && cells != null;
    }

    /// <summary>
    /// 특정 스크린 좌표에 있는 셀을 리턴하는 함수
    /// </summary>
    /// <param name="screen">스크린 좌표</param>
    /// <returns>셀이 없으면 null, 그 외에는 스크린 좌표에 있는 셀</returns>
    Cell GetCell(Vector2 screen)
    {
        Cell result = null;
        Vector2Int grid = ScreenToGrid(screen);
        int? index = GridToIndex(grid.x, grid.y);
        if( index != null )
        {
            result = cells[index.Value];
        }

        return result;
    }

    // 입력 처리용 함수들 ---------------------------------------------------------------------------------
    private void OnLeftPress(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        //Debug.Log( GetCell(screen)?.gameObject.name );

        Cell cell = GetCell(screen);
        if(cell != null )
        {
            gameManager.GameStart();
            cell.LeftPress();

            if (gameManager.IsPlaying)
            {
                onBoardLeftPress?.Invoke();
            }
        }
    }

    private void OnLeftRelease(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        Cell cell = GetCell(screen);
        cell?.LeftRelease();
        if (gameManager.IsPlaying)
        {
            onBoardLeftRelease?.Invoke();
        }
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
        
        // 이 위치에 있는 셀의 RightPress()가 실행된다.
        Cell cell = GetCell(screen);
        if( cell != null )
        {
            gameManager.GameStart();
            cell.RightPress();
        }
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        if(Mouse.current.leftButton.isPressed)  // 마우스 왼쪽버튼 눌려진 상태 확인
        {            
            Vector2 screen = context.ReadValue<Vector2>();
            Cell cell = GetCell(screen);
            CurrentCell = cell;
        }

    }

    // 기타 유틸리티 함수들 ------------------------------------------------------------------------------
    
    /// <summary>
    /// 셔플용 함수
    /// </summary>
    /// <param name="count">셔플할 숫자 범위(0 ~ count-1)</param>
    /// <param name="result">셔플된 결과</param>
    void Shuffle(int count, out int[] result)
    {
        // count만큼 순서대로 숫자가 들어간 배열 만들기
        result = new int[count];
        for( int i = 0; i < count; i++ )
        {
            result[i] = i;
        }

        // 위에서 만든 배열을 섞기
        int loopCount = result.Length - 1;
        for (int i = 0; i < loopCount; i++) // 8*8일 때 63번 반복
        {
            int randomIndex = UnityEngine.Random.Range( 0, result.Length - i ); // 처음에는 0~63 중 랜덤으로 선택
            int lastIndex = loopCount - i;                                      // 처음에는 63

            // 랜덤으로 고른 것과 마지막을 스왑
            (result[lastIndex], result[randomIndex]) = (result[randomIndex], result[lastIndex]);    
        }
    }

    /// <summary>
    /// 특정 셀의 주변 셀을 돌려주는 함수
    /// </summary>
    /// <param name="id">중심 셀의 아이디</param>
    /// <returns>중심 셀의 주변 셀들의 리스트</returns>
    public List<Cell> GetNeightbors(int id)
    {
        List<Cell> result = new List<Cell>();
        Vector2Int grid = IndexToGrid( id );    // id의 그리드 위치를 x와 y 모두 +-1씩해서 구하기
        for (int y = -1; y < 2; y++)
        {
            for(int x = -1; x < 2; x++)
            {
                if(!(x == 0 && y == 0))         // 자기 자신은 제외
                {
                    int? index = GridToIndex(x + grid.x, y + grid.y);   
                    if(index != null)           // valid한 id면 추가
                    {
                        result.Add(cells[index.Value]);
                    }
                }
            }
        }

        return result;
    }

#if UNITY_EDITOR
    public void Test_OpenAllCover()
    {
        foreach( Cell cell in cells )
        {
            cell.Test_OpenCover();
        }
    }

    public void Test_BoardReset()
    {
        ResetBoard();
    }
#endif
}
