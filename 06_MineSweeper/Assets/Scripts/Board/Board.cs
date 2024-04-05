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
    /// 인풋시스템을 위한 인풋액션
    /// </summary>
    PlayerInputActions inputActions;

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
                GameObject cellObj = Instantiate(cellPrefab, transform);
                Cell cell = cellObj.GetComponent<Cell>();
                cell.transform.localPosition = new Vector3(x * Distance, -y * Distance);

                int id = x + y * width;
                cells[id] = cell;
                cellObj.name = $"Cell_{id}_({x},{y})";
            }
        }

        ResetBoard();
    }

    /// <summary>
    /// 보드에 존재하는 모들 셀의 데이터를 리셋하고 지뢰를 새로 배치하는 함수(게임 재시작용 함수)
    /// </summary>
    void ResetBoard()
    {

    }

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
    /// 지정된 그리드 좌표가 보드 내부인지 확인하는 함수
    /// </summary>
    /// <param name="x">x좌표</param>
    /// <param name="y">y좌표</param>
    /// <returns>보드 안이면 true, 밖이면 false</returns>
    bool IsValidGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
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
        Debug.Log( GetCell(screen).gameObject.name );
    }

    private void OnLeftRelease(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
    }

    private void OnRightClick(InputAction.CallbackContext context)
    {
        Vector2 screen = Mouse.current.position.ReadValue();
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector2 screen = context.ReadValue<Vector2>();
    }
}
