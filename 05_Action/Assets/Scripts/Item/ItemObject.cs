using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : RecycleObject
{
    ItemData data = null;
    SpriteRenderer spriteRenderer = null;

    public ItemData ItemData
    {
        get => data;
        set
        {
            if( data == null )  // 활성화 이후에는 단 한번만 설정 가능(팩토리에서 처리해야 함)
            {
                data = value;
                spriteRenderer.sprite = data.itemIcon;  // 아이콘 변경                
            }
        } 
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnEnable()
    {
        data = null;
        base.OnEnable();
    }

    /// <summary>
    /// 아이템을 비활성화 시키는 함수
    /// </summary>
    public void End()
    {
        gameObject.SetActive(false);
    }
}
