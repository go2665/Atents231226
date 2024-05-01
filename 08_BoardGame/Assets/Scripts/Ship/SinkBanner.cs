using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkBanner : MonoBehaviour
{
    Transform background;
    Transform letter1;
    Transform letter2;

    private void Awake()
    {
        background = transform.GetChild(0);
        letter1 = transform.GetChild(1);
        letter2 = transform.GetChild(2);
    }

    public void Open(Ship ship)
    {
        transform.rotation = Quaternion.Euler(0, (int)ship.Direction * 90, 0);

        // 배경 크기와 위치 조정
        background.localScale = new Vector3(1, ship.Size, 1);
        background.localPosition = new Vector3(0, 0, 0.5f + -0.5f * ship.Size);

        // 글자가 바로 보이게 돌리기
        letter1.localRotation = Quaternion.Euler(90, 0, (int)ship.Direction * 90);
        letter2.localRotation = Quaternion.Euler(90, 0, (int)ship.Direction * 90);

        Vector3 letter1Pos = Vector3.zero;
        Vector3 letter2Pos = Vector3.zero;

        // 글자 방향 확인
        Vector3 dir = Vector3.zero; // 배 머리 -> 꼬리 방향
        switch (ship.Direction)
        {
            case ShipDirection.North:
                dir.x = 0;
                dir.y = 0;
                dir.z = -1;
                break;
            case ShipDirection.East:
                dir.x = -1;
                dir.y = 0;
                dir.z = 0;
                break;
            case ShipDirection.South:
                dir.x = 0;
                dir.y = 0;
                dir.z = 1;
                break;
            case ShipDirection.West:
                dir.x = 1;
                dir.y = 0;
                dir.z = 0;
                break;         
        }

        switch (ship.Size)
        {
            case 2:
                letter1Pos = letter1.position + dir * 0.0f;
                letter2Pos = letter2.position + dir * 0.0f;
                break;
            case 3:
                letter1Pos = letter1.position + dir * 0.5f;
                letter2Pos = letter1.position + dir * 1.5f;
                break;
            case 4:
                letter1Pos = letter1.position + dir * 0.5f;
                letter2Pos = letter1.position + dir * 2.5f;
                break;
            case 5:
                letter1Pos = letter1.position + dir * 1.0f;
                letter2Pos = letter1.position + dir * 3.0f;
                break;
        }

        // 글이 역순일 경우 위치 스왑
        if( ship.Direction == ShipDirection.East || ship.Direction == ShipDirection.South )
        {
            (letter1Pos, letter2Pos) = (letter2Pos, letter1Pos);
        }

        // 계산한 위치 적용
        letter1.position = letter1Pos;
        letter2.position = letter2Pos;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
