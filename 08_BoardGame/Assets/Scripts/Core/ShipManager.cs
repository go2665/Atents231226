using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : Singleton<ShipManager>
{
    /// <summary>
    /// 함선 프리팹(모델 정보 없음)
    /// </summary>
    public GameObject shipPrefab;

    /// <summary>
    /// 함선 모델 프리팹(메시만 가지고 있음)
    /// </summary>
    public GameObject[] shipModels;

    /// <summary>
    /// 함선의 원본 머티리얼(인스팩터에서 설정)
    /// </summary>
    public Material[] shipMaterials;

    /// <summary>
    /// 일반적인 배의 머티리얼(불투명)
    /// </summary>
    public Material NormalShipMaterial => shipMaterials[0];

    /// <summary>
    /// 배치모드인 배의 머티리얼(반투명)
    /// </summary>
    public Material DeplyModeShipMaterial => shipMaterials[1];

    /// <summary>
    /// 배치모드일때 배치가 가능한 경우의 색상
    /// </summary>
    readonly Color SuccessColor = new Color(0, 1, 0, 0.2f);

    /// <summary>
    /// 배치모드일때 배치가 불가능한 경우의 색상
    /// </summary>
    readonly Color FailColor = new Color(1, 0, 0, 0.2f);

    /// <summary>
    /// 배의 종류 개수
    /// </summary>
    int shipTypeCount;

    /// <summary>
    /// 배의 종류 개수 확인용 프로퍼티
    /// </summary>
    public int ShipTypeCount => shipTypeCount;

    /// <summary>
    /// 배의 방향 개수
    /// </summary>
    int shipDirectionCount;

    /// <summary>
    /// 배의 방향 개수 확인용 프로퍼티
    /// </summary>
    public int ShipDirectionCount => shipDirectionCount;

    /// <summary>
    /// 쉐이더 접근용 이름을 아이디로 변환해 놓은 것
    /// </summary>
    readonly int BaseColor_ID = Shader.PropertyToID("_BaseColor");

    protected override void OnInitialize()
    {
        // 개수 미리 계산해 두기
        shipTypeCount = Enum.GetValues(typeof(ShipType)).Length - 1;
        shipDirectionCount = Enum.GetValues(typeof(ShipDirection)).Length;
    }

    /// <summary>
    /// 배를 만드는 함수
    /// </summary>
    /// <param name="shipType">생성할 함선의 종류</param>
    /// <param name="ownerPlayer">생성된 배를 가지는 플레이어의 트랜스폼</param>
    /// <returns>생성 완료된 배</returns>
    public Ship MakeShip(ShipType shipType, Transform ownerPlayer)
    {
        GameObject shipObj = Instantiate(shipPrefab, ownerPlayer);  // 배 만들고
        GameObject modelPrefab = GetShipModel(shipType);            // 모델 가져와서
        Instantiate(modelPrefab, shipObj.transform);                // 모델 만들고 배 아래에 붙이기

        Ship ship = shipObj.GetComponent<Ship>();
        ship.Initialize(shipType);  // 배 초기화

        return ship;
    }  
    
    /// <summary>
    /// 함선의 모델 프리팹을 돌려주는 함수
    /// </summary>
    /// <param name="type">함선의 종류</param>
    /// <returns>함선 프리팹</returns>
    GameObject GetShipModel(ShipType type)
    {
        return shipModels[(int)type - 1];
    }

    /// <summary>
    /// 배치모드 머티리얼의 색상을 지정하는 함수
    /// </summary>
    /// <param name="isSuccess">배치 가능한지 안한지 여부</param>
    public void SetDeployModeColor(bool isSuccess)
    {
        if(isSuccess)   // 배치 가능 여부에 따라 색상 변경
        {
            DeplyModeShipMaterial.SetColor(BaseColor_ID, SuccessColor);
        }
        else
        {
            DeplyModeShipMaterial.SetColor(BaseColor_ID, FailColor);
        }

    }
}
