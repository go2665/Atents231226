using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    TextMeshPro text;   // 3D 글자(UI아님)

    public void Use()
    {
        Open();
    }
}

// 실습
// 1. 일정 시간 이후에 자동으로 문이 닫히기
// 2. 플레이어가 자신의 트리거 안에 들어오면 글자로 단축키 보여주기