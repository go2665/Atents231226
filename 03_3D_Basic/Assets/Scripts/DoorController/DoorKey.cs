using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public DoorBase target;
}

// 이 오브젝트와 플레이어가 닿으면 target에 지정된 문이 열린다.
// 이 오브젝트와 플레이어가 닿으면 이 오브젝트는 사라진다.
// 이 오ㅡ젝트는 제자리에서 빙글빙글 돈다.
