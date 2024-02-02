using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoLock : DoorAuto
{
    bool locking = false;
    bool Locking
    {
        get => locking;
        set
        {
            if( locking != value )
            {
                locking = value;
                if( locking )
                {
                    // 잠그기
                    doorMaterial.color = lockColor;
                    sensor.enabled = false;
                }
                else
                {
                    // 잠금 해제하기
                    doorMaterial.color = unlockColor;
                    sensor.enabled = true;
                }
            }
        }
    }

    BoxCollider sensor;

    public Color lockColor;
    public Color unlockColor;

    Material doorMaterial;

    protected override void Awake()
    {
        base.Awake();
        sensor = GetComponent<BoxCollider>();

        Transform door = transform.GetChild(1);
        door = door.GetChild(0);    // 문 찾기

        MeshRenderer meshRenderer = door.GetComponent<MeshRenderer>();  // 문의 랜더러 찾기
        doorMaterial = meshRenderer.material;   // 랜더러에서 머티리얼 가져오기
    }

    protected override void Start()
    {        
        Locking = true;
    }

    protected override void OnKeyUsed()
    {
        Locking = false;    // 열쇠가 소비되었을 때 잠금이 해제된다.
    }
}
