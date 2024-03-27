using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 코드(ID)
/// </summary>
public enum ItemCode : byte
{
    Ruby = 0,
    Emerald,
    Sapphire,
    CopperCoin,
    SilverCoin,
    GoldCoin,
    Apple,
    Bread,
    Beer,
    HealingPotion,
    ManaPotion, 
    IronSword,
    SilverSword,
    OldSword,
    KiteShield,
    RoundShield
}

/// <summary>
/// 아이템 정렬 기준
/// </summary>
public enum ItemSortBy : byte
{
    Code = 0,   // 코드 기준
    Name,       // 이름 기준
    Price       // 가격 기준
}

/// <summary>
/// 장비 가능한 아이템의 종류
/// </summary>
public enum EquipType : byte
{
    Weapon,
    Shield
}