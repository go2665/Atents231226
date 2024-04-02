using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    /// <summary>
    /// 노이즈 반지름
    /// </summary>
    public float noisePower = 0.5f;

    ItemPool itemPool;
    HitEffectPool hitEffectPool;
    EnemyPool enemyPool;
    DamageTextPool damageTextPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        itemPool = GetComponentInChildren<ItemPool>();
        if(itemPool != null ) itemPool.Initialize();

        hitEffectPool = GetComponentInChildren<HitEffectPool>();
        if(hitEffectPool != null ) hitEffectPool.Initialize();

        enemyPool = GetComponentInChildren<EnemyPool>();
        if (enemyPool != null) enemyPool.Initialize();

        damageTextPool = GetComponentInChildren<DamageTextPool>();
        if (damageTextPool != null) damageTextPool.Initialize();
    }

    /// <summary>
    /// 슬라임 하나 가져오는 함수
    /// </summary>
    /// <returns>배치된 슬라임 하나</returns>
    public Enemy GetEnemy()
    {
        return enemyPool.GetObject();
    }

    /// <summary>
    /// 슬라임 하나를 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 슬라임 하나</returns>
    public Enemy GetEnemy(Vector3 position, float angle = 0.0f)
    {
        return enemyPool.GetObject(position, angle * Vector3.forward);
    }

    /// <summary>
    /// 슬라임 하나를 특정 웨이포인트를 사용하고, 특정 위치에, 특정 각도로 배치
    /// </summary>
    /// <param name="index">사용할 웨이포인트의 인덱스</param>
    /// <param name="position">배치될 위치</param>
    /// <param name="angle">배치 될 때의 각도</param>
    /// <returns>배치된 슬라임 하나</returns>
    public Enemy GetEnemy(int index, Vector3 position, float angle = 0.0f)
    {
        return enemyPool.GetObject(index, position, angle * Vector3.forward);
    }

    /// <summary>
    /// 아이템을 하나 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <returns>아이템의 게임 오브젝트</returns>
    public GameObject MakeItem(ItemCode code)
    {
        ItemData data = GameManager.Instance.ItemData[code];    // 아이템 데이터 받아오고
        ItemObject obj = itemPool.GetObject();
        obj.ItemData = data;                    // 풀에서 하나 꺼내고 데이터 설정

        return obj.gameObject;
    }

    /// <summary>
    /// 아이템을 하나 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="useNoise">위치에 노이즈를 적용할지 여부(true면 적용, false면 안함)</param>
    /// <returns>아이템의 게임 오브젝트</returns>
    public GameObject MakeItem(ItemCode code, Vector3 position, bool useNoise = false)
    {
        GameObject obj = MakeItem(code);
        Vector3 noise = Vector3.zero;
        if( useNoise )
        {
            Vector2 rand = Random.insideUnitCircle * noisePower;
            noise.x = rand.x;
            noise.z = rand.y;
        }
        obj.transform.position = position + noise;

        return null;
    }

    /// <summary>
    /// 아이템을 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="count">생성할 개수</param>
    /// <returns>아이템의 게임 오브젝트들</returns>
    public GameObject[] MakeItems(ItemCode code, uint count)
    {
        GameObject[] items = new GameObject[count];
        for(int i = 0;i<count;i++)
        {
            items[i] = MakeItem(code);
        }
        return items;
    }

    /// <summary>
    /// 아이템을 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <param name="count">생성할 개수</param>
    /// <param name="position">생성할 위치</param>
    /// <param name="useNoise">위치에 노이즈를 적용할지 여부(true면 적용, false면 안함)</param>
    /// <returns>아이템의 게임 오브젝트들</returns>
    public GameObject[] MakeItems(ItemCode code, uint count, Vector3 position, bool useNoise = false)
    {
        GameObject[] items = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            items[i] = MakeItem(code, position, useNoise);
        }
        return items;
    }

    /// <summary>
    /// 히트 이팩트를 생성하는 함수
    /// </summary>
    /// <param name="position">생성될 위치</param>
    /// <returns></returns>
    public GameObject GetHitEffect(Vector3? position)
    {
        return hitEffectPool.GetObject(position).gameObject;
    }

    /// <summary>
    /// 데미지 텍스트를 생성하는 함수
    /// </summary>
    /// <param name="damage">설정될 데미지</param>
    /// <returns></returns>
    public GameObject GetDamageText(int damage, Vector3? position)
    {
        return damageTextPool.GetObject(damage, position);
    }
}