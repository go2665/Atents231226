using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 배의 종류
/// </summary>
public enum ShipType : byte
{
    None = 0,
    Carrier,    // 항공모함(5칸)
    BattleShip, // 전함(4칸)
    Destroyer,  // 구축함(3칸)
    Submarine,  // 잠수함(3칸)
    PatrolBoat  // 경비정(2칸)
}

/// <summary>
/// 배의 방향(뱃머리가 바라보는 방향)
/// </summary>
public enum ShipDirection : byte
{
    North = 0,
    East,
    South,
    West
}

public class Ship : MonoBehaviour
{
    /// <summary>
    /// 배 종류
    /// </summary>
    ShipType shipType = ShipType.None;

    /// <summary>
    /// 배 종류 확인 및 설정용 프로퍼티
    /// </summary>
    public ShipType Type
    {
        get => shipType;
        private set
        {
            shipType = value;
            switch (shipType)   // 배 종류 별로 크기와 이름을 설정한다.
            {
                case ShipType.Carrier:
                    size = 5;
                    shipName = "항공모함";
                    break;
                case ShipType.BattleShip:
                    size = 4;
                    shipName = "전함";
                    break;
                case ShipType.Destroyer:
                    size = 3;
                    shipName = "구축함";
                    break;
                case ShipType.Submarine:
                    size = 3;
                    shipName = "잠수함";
                    break;
                case ShipType.PatrolBoat:
                    size = 2;
                    shipName = "경비정";
                    break;
            }
        }
    }

    /// <summary>
    /// 배의 이름
    /// </summary>
    string shipName = string.Empty;

    /// <summary>
    /// 배 이름 확인용 프로퍼티
    /// </summary>
    public string ShipName => shipName;

    /// <summary>
    /// 배의 크기(=최대 HP)
    /// </summary>
    int size = 0;

    /// <summary>
    /// 패의 크기 확인용 프로퍼티
    /// </summary>
    public int Size => size;

    /// <summary>
    /// 배의 현재 내구도
    /// </summary>
    int hp = 0;
    public int HP
    {
        get => hp;
        private set
        {
            hp = value;
            if(hp < 1)          // hp가 0이하가 되면 
            {
                OnSinking();    // 침몰한다.
            }
        }
    }

    /// <summary>
    /// HP가 0보다 크면 살아있다.
    /// </summary>
    bool IsAlive => hp > 0;


    /// <summary>
    /// 배가 바라보는 방향. (북동남서로 회전하는 것이 정방향)
    /// </summary>
    ShipDirection direction = ShipDirection.North;
    public ShipDirection Direction
    {
        get => direction;
        set
        {
            direction = value;
            modelRoot.rotation = Quaternion.Euler(0, (int)direction * 90.0f, 0);    // 방향이 변경되면 방향에 맞게 회전
        }
    }

    /// <summary>
    /// 배의 모델 메시의 트랜스폼
    /// </summary>
    Transform modelRoot;

    /// <summary>
    /// 배가 배치된 위치(그리드 좌표)
    /// </summary>
    Vector2Int[] positions;

    /// <summary>
    /// 배가 배치된 위치 확인용 프로퍼티
    /// </summary>
    public Vector2Int[] Positions => positions;

    /// <summary>
    /// 배의 배치 여부(true면 배치되었고, false면 배치되지 않았다)
    /// </summary>
    bool isDeployed = false;

    /// <summary>
    /// 배의 배치 여부 확인용 프로퍼티
    /// </summary>
    public bool IsDeployed => isDeployed;

    /// <summary>
    /// 배의 머티리얼을 변경하기 위해 찾아 놓은 랜더러
    /// </summary>
    Renderer shipRenderer;

    /// <summary>
    /// 함선이 배치되거나 배치 해제 되었을 때를 알리는 델리게이트(bool:true면 배치되었다. false면 배치 해제 되었다.)
    /// </summary>
    public Action<bool> onDeploy;

    /// <summary>
    /// 함선이 공격을 당했을 때를 알리는 델리게이트(Ship: 자기자신. 이름이나 종류등에 대한 접근이 필요함)
    /// </summary>
    public Action<Ship> onHit;

    /// <summary>
    /// 함선이 침몰되었음을 알리는 델리게이트(Ship:자기 자신.)
    /// </summary>
    public Action<Ship> onSink;

    /// <summary>
    /// 함선 방향 개수
    /// </summary>
    int shipDirectionCount;

    /// <summary>
    /// 함선이 침몰했을 때 보일 배너
    /// </summary>
    SinkBanner sinkBanner;

    /// <summary>
    /// 배 초기화용 함수
    /// </summary>
    /// <param name="shipType">배의 종류</param>
    public void Initialize(ShipType shipType)
    {
        Type = shipType;    // 종류 결정
        HP = Size;          // HP 종류에 맞게 설정

        sinkBanner = GetComponentInChildren<SinkBanner>();
        sinkBanner.Close();

        modelRoot = transform.GetChild(1);
        shipRenderer = modelRoot.GetComponentInChildren<Renderer>();

        ResetData();

        gameObject.name = $"{Type}_{Size}";
        gameObject.SetActive(false);

        shipDirectionCount = ShipManager.Instance.ShipDirectionCount;
    }

    /// <summary>
    /// 공통적으로 데이터 초기화하는 함수
    /// </summary>
    void ResetData()
    {        
        Direction = ShipDirection.North;
        isDeployed = false;
        positions = null;
    }


    /// <summary>
    /// 함선의 머티리얼을 선택하는 함수
    /// </summary>
    /// <param name="isNormal">true면 불투명한 머티리얼, false면 배치모드용 반투명 머티리얼</param>
    public void SetMaterialType(bool isNormal = true)
    {
        if(isNormal)
        {
            shipRenderer.material = ShipManager.Instance.NormalShipMaterial;
        }
        else
        {
            shipRenderer.material = ShipManager.Instance.DeplyModeShipMaterial;
        }
    }

    /// <summary>
    /// 함선이 배치되었을 때의 처리를 하는 함수
    /// </summary>
    /// <param name="deployPositions">배치되는 위치들</param>
    public void Deploy(Vector2Int[] deployPositions)
    {
        SetMaterialType();              // 머티리얼을 정상으로 돌리기
        isDeployed = true;              // 배치되었다고 표시
        positions = deployPositions;    // 배치된 위치(그리드 좌표) 기록
    }

    /// <summary>
    /// 함선이 배치 해제되었을 때의 처리를 하는 함수
    /// </summary>
    public void UnDeploy()
    {
        ResetData();                    // 모든 데이터 초기화
    }

    /// <summary>
    /// 함선을 90도씩 회전 시키는 함수
    /// </summary>
    /// <param name="isCW">true면 시계방향, false면 반시계방향</param>
    public void Rotate(bool isCW = true)
    {
        // 회전방향에 따라 방향 +-1
        if(isCW)
        {
            Direction = (ShipDirection)(((int)Direction + 1) % shipDirectionCount);
        }
        else
        {
            // 음수 %연산이 일어나는 것을 방지
            // %연산을 할 때는 나누는 숫자를 몇번을 더해도 결과에 영향을 주지 않는다.
            Direction = (ShipDirection)(((int)Direction + (shipDirectionCount - 1)) % shipDirectionCount);
        }
    }

    /// <summary>
    /// 함선을 랜덤한 방향으로 회전시키는 함수
    /// </summary>
    public void RandomRotate()
    {
        int rotateCount = UnityEngine.Random.Range(0, shipDirectionCount);  // 0~3 중 하나를 랜덤으로 정하기
        for(int i=0;i<rotateCount; i++)
        {
            Rotate();   // 그만큼 회전 시키기
        }
    }

    /// <summary>
    /// 함선이 공격 받았을 때 실행되는 함수
    /// </summary>
    public void OnHitted()
    {
        Debug.Log($"{ShipName} 명중");

        onHit?.Invoke(this);
        HP--;
    }

    /// <summary>
    /// 배가 침몰할 때 실행될 함수
    /// </summary>
    void OnSinking()
    {
        Debug.Log($"{ShipName} 침몰");
        sinkBanner.Open(this);
        onSink?.Invoke(this);
    }

#if UNITY_EDITOR

    public void Test_SinkBanner()
    {
        sinkBanner.Open(this);
    }
#endif

}
