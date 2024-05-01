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

    private void Start()
    {
        Ship ship = GetComponentInParent<Ship>();
        ship.onSink += Open;

        Close();
    }

    // 침몰 배너 열기
    void Open(Ship ship)
    {
        transform.rotation = Quaternion.Euler(0, (int)ship.Direction * 90, 0);

        // 배경 크기와 위치 조정
        background.localScale = new Vector3(1, ship.Size, 1);
        background.localPosition = new Vector3(0, 0, 0.5f + -0.5f * ship.Size);

        // 글자가 바로 보이게 돌리기        
        letter1.localRotation = Quaternion.Euler(90, 0, (int)ship.Direction * 90);
        letter2.localRotation = Quaternion.Euler(90, 0, (int)ship.Direction * 90);

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

        // 글자 위치 초기화
        letter1.localPosition = Vector3.up * 0.01f;
        letter2.localPosition = Vector3.up * 0.01f;

        // 글자 위치 계산하기
        float diff = (ship.Size - 2) * 0.25f;
        Vector3 letter1Pos = letter1.position + dir * diff;
        Vector3 letter2Pos = letter1.position + dir * (ship.Size - 1.0f - diff);

        // 글이 역순일 경우 위치 스왑
        if( ship.Direction == ShipDirection.East || ship.Direction == ShipDirection.South )
        {
            (letter1Pos, letter2Pos) = (letter2Pos, letter1Pos);
        }

        // 계산한 위치 적용
        letter1.position = letter1Pos;
        letter2.position = letter2Pos;

        // 보이게 만들기
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 침몰 배너 닫기
    /// </summary>
    void Close()
    {
        gameObject.SetActive(false);
    }
}
