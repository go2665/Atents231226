using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBase
{
    [Header("보스 데이터")]
    /// <summary>
    /// 보스의 총알
    /// </summary>
    public PoolObjectType bullet = PoolObjectType.EnemyBossBullet;

    /// <summary>
    /// 보스의 미사일
    /// </summary>
    public PoolObjectType misslie = PoolObjectType.EnemyBossMisslie;

    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float bulletInterval = 1.0f;

    /// <summary>
    /// 보스 활동영역(최소, 월드좌표)
    /// </summary>
    public Vector2 areaMin = new Vector2(2, -3);

    /// <summary>
    /// 보스 활동영역(최대, 월드좌표)
    /// </summary>
    public Vector2 areaMax = new Vector2(7, 3);

    /// <summary>
    /// 미사일 일제사격 회수
    /// </summary>
    public int barrageCount = 3;

    /// <summary>
    /// 총알 발사 위치1 
    /// </summary>
    Transform fire1;

    /// <summary>
    /// 총알 발사 위치2
    /// </summary>
    Transform fire2;

    /// <summary>
    /// 미사일 발사 위치
    /// </summary>
    Transform fire3;

    /// <summary>
    /// 보스의 이동 방향
    /// </summary>
    Vector3 moveDirection = Vector3.left;

    private void Awake()
    {
        Transform fireTransforms = transform.GetChild(1);
        fire1 = fireTransforms.GetChild(0);
        fire2 = fireTransforms.GetChild(1);
        fire3 = fireTransforms.GetChild(2);
    }
}

// 0. 스폰되면 정해진 영역의 가운데까지 전진
// 1. 특정 영역 안에서 위아래로 왕복한다.
// 2. 계속 주기적으로 총알을 발사한다.(1번 시작할 때 부터)
// 3. 이동 방향을 변경할 때 미사일을 3발 연속으로 발사한다.