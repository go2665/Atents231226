using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 코드(ID)
/// </summary>
public enum ItemCode
{
    Ruby = 0,
    Emerald,
    Sapphire
}

/// <summary>
/// 아이템 정렬 기준
/// </summary>
public enum ItemSortBy
{
    Code,       // 코드 기준
    Name,       // 이름 기준
    Price       // 가격 기준
}