using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
    // 사정 거리
    // 탄창 (탄창 크기, 탄창에 남아있는 총알 수)
    // 데미지
    // 연사 속도
    // 탄 퍼짐
    // 총 반동

    /// <summary>
    /// 총의 사정거리
    /// </summary>
    public float range;

    /// <summary>
    /// 탄창 크기
    /// </summary>
    public int clipSize;

    /// <summary>
    /// 장전된 총알 수(=남은 총알 수)
    /// </summary>
    int bulletCount;

    /// <summary>
    /// 총알 개수의 변경 및 확인을 위한 프로퍼티
    /// </summary>
    protected int BulletCount
    {
        get => bulletCount;
        set
        {
            bulletCount = value;
            onBulletCountChange?.Invoke(bulletCount);   // 총알 개수가 변경되었다고 알림
        }
    }

    /// <summary>
    /// 총의 데미지(총알 한발당 데미지)
    /// </summary>
    public float damage;

    /// <summary>
    /// 총의 연사속도
    /// </summary>
    public float fireRate;

    /// <summary>
    /// 탄 퍼지는 각도
    /// </summary>
    public float spread;

    /// <summary>
    /// 총의 반동
    /// </summary>
    public float recoil;

    /// <summary>
    /// 총알이 발사되는 트랜스폼(플레이어의 카메라 루트 위치)
    /// </summary>
    protected Transform fireTransform;

    /// <summary>
    /// 남은 총알 개수가 변경되었음을 알리는 델리게이트(int:남은 총알 개수)
    /// </summary>
    public Action<int> onBulletCountChange;

    /// <summary>
    /// 총알이 한발 발사되었음을 알리는 델리게이트(float:반동 정도)
    /// </summary>
    public Action<float> onFire;

    void Shoot()
    {
    }

    void Reload()
    {

    }

    public void Equip()
    {
        fireTransform = GameManager.Instance.Player.FireTransform;
    }

    public void UnEquip()
    {

    }

    private void OnDrawGizmos()
    {
        
    }

}
