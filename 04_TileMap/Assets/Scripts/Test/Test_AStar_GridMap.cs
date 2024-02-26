using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AStar_GridMap : TestBase
{
    public int width = 7;
    public int height = 7;

    public Vector2Int start;
    public Vector2Int end;

    GridMap gridMap;

    private void Start()
    {
        gridMap = new GridMap(width, height);

        Node node;
        node = gridMap.GetNode(gridMap.IndexToGrid(17));
        node.nodeType = Node.NodeType.Wall;

        node = gridMap.GetNode(gridMap.IndexToGrid(24));
        node.nodeType = Node.NodeType.Wall;

        node = gridMap.GetNode(gridMap.IndexToGrid(31));
        node.nodeType = Node.NodeType.Wall;

        node = gridMap.GetNode(gridMap.IndexToGrid(37));
        node.nodeType = Node.NodeType.Wall;

        node = gridMap.GetNode(gridMap.IndexToGrid(38));
        node.nodeType = Node.NodeType.Wall;

        start = gridMap.IndexToGrid(22);
        end = gridMap.IndexToGrid(34);
    }

    void PrintList(List<Vector2Int> list)
    {
        string str = "";
        foreach (Vector2Int v in list)
        {
            str += $"{v} -> ";
        }
        Debug.Log(str + "End");
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        List<Vector2Int> path = AStar.PathFind(gridMap, start, end);
        PrintList(path);
    }

}
