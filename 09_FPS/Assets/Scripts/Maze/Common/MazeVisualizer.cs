using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : MonoBehaviour
{
    /// <summary>
    /// 셀의 프리팹
    /// </summary>
    public GameObject cellPrefab;

    /// <summary>
    /// 골의 프리팹
    /// </summary>
    public GameObject goalPrefab;

    /// <summary>
    /// 파라메터로 받은 미로를 그리는 함수
    /// </summary>
    /// <param name="maze">Maze로 만들어진 미로</param>
    public void Draw(Maze maze)
    {
        float size = CellVisualizer.CellSize;
        foreach(var cell in maze.Cells)
        {
            GameObject obj = Instantiate(cellPrefab, transform);
            obj.transform.Translate(cell.X * size, 0, -cell.Y * size);
            obj.gameObject.name = $"Cell_({cell.X}, {cell.Y})";

            CellVisualizer cellVisualizer = obj.GetComponent<CellVisualizer>();
            cellVisualizer.RefreshWall(cell.Path);
        }

        GameObject goalObj = Instantiate(goalPrefab, transform);
        Goal goal = goalObj.GetComponent<Goal>();
        goal.SetRandomPosition(maze.Width, maze.Height);

        Debug.Log("미로 비주얼라이저 그리기 완료");
    }

    /// <summary>
    /// 모든 셀을 제거하는 함수
    /// </summary>
    public void Clear()
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }
        Debug.Log("미로 비주얼라이저 초기화");
    }

    /// <summary>
    /// 그리드 좌표로 셀의 월드 좌표 구하는 함수
    /// </summary>
    /// <param name="x">x위치</param>
    /// <param name="y">y위치</param>
    /// <returns></returns>
    public static Vector3 GridToWorld(int x, int y)
    {
        float size = CellVisualizer.CellSize;
        float sizeHalf = size * 0.5f;

        return new(size * x + sizeHalf, 0, size * -y - sizeHalf);
    }

    /// <summary>
    /// 월드 좌표를 그리드 좌표로 변경하는 함수
    /// </summary>
    /// <param name="world">월드 좌표</param>
    /// <returns>미로상의 그리드 좌표</returns>
    public static Vector2Int WorldToGrid(Vector3 world)
    {
        float size = CellVisualizer.CellSize;
        Vector2Int result = new((int)(world.x / size), (int)(-world.z / size));
        return result;
    }
}
