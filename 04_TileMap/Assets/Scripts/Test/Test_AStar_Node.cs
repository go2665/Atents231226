using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AStar_Node : TestBase
{
    private void Start()
    {
        //Node node = new Node();

        Node node1 = new Node(10,20);
        Node node2 = new Node(10,20);

        List<Node> nodes = new List<Node>();
        nodes.Sort();

        //int i = 0;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        List<int> list = new List<int>();
        list.Add(3);
        list.Add(5);
        list.Add(2);
        list.Add(4);
        list.Add(6);
        list.Add(1);

        list.Sort();

        //int i = 0;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Node node1 = new Node(10, 20);
        node1.G = 10;
        node1.H = 0;
        Node node2 = new Node(10, 20);
        node2.G = 50;
        node2.H = 0;
        Node node3 = new Node(10, 20);
        node3.G = 20;
        node3.H = 0;
        Node node4 = new Node(10, 20);
        node4.G = 40;
        node4.H = 0;
        Node node5 = new Node(10, 20);
        node5.G = 30;
        node5.H = 0;

        List<Node> nodes = new List<Node>();
        nodes.Add(node1);
        nodes.Add(node2);
        nodes.Add(node3);
        nodes.Add(node4);
        nodes.Add(node5);
        nodes.Sort();
        //int i = 0;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Node node1 = new Node(10, 20);
        Node node2 = new Node(10, 20);

        Vector2Int v1 = new Vector2Int(10, 20);

        if( node1 == node2 )    // 위치가 같은지 다른지 확인을 하고 싶다.
        {
            Debug.Log("같다.");
        }
        else
        {
            Debug.Log("다르다.");
        }

        if( node1 == v1 )
        {
            Debug.Log("같다.");
        }
        else
        {
            Debug.Log("다르다.");
        }

        //if( v1 == node1 ) // 안됨
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // TestData 정렬 확인하기

        TestData t1 = new TestData(0, 5.0f, "b");
        TestData t2 = new TestData(2, 1.0f, "d");
        TestData t3 = new TestData(1, 4.0f, "a");
        TestData t4 = new TestData(3, 2.0f, "c");
        TestData t5 = new TestData(4, 3.0f, "e");

        List<TestData> nodes = new List<TestData>(5);
        nodes.Add(t1);
        nodes.Add(t2);
        nodes.Add(t3);
        nodes.Add(t4);
        nodes.Add(t5);

        nodes.Sort();

        //int i = 0;
        Debug.Log("4");
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
    }
}
