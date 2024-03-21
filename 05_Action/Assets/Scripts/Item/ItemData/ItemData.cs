using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 한 종류의 정보를 저장하는 스크립터블 오브젝트
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order = 0)]
public class ItemData : ScriptableObject
{
    [Header("아이템 기본 정보")]
    public ItemCode code;
    public string itemName = "아이템";
    public string itemDescription = "설명";
    public Sprite itemIcon;
    public uint price = 0;
    public uint maxStackCount = 1;
    //public GameObject modelPrefab;
}
