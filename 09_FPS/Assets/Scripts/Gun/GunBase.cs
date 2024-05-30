using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunBase : MonoBehaviour
{
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
    /// 총의 연사속도(1초당 발사 수)
    /// </summary>
    public float fireRate;

    /// <summary>
    /// 현재 발사가 가능한지 여부를 확인하는 변수
    /// </summary>
    protected bool isFireReady = true;

    /// <summary>
    /// 탄 퍼지는 각도의 절반
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
    /// Muzzle 이팩트 발동용 ID 
    /// </summary>
    readonly int onFireID = Shader.PropertyToID("OnFire");

    /// <summary>
    /// Muzzle 이팩트
    /// </summary>
    VisualEffect muzzleEffect;

    /// <summary>
    /// 남은 총알 개수가 변경되었음을 알리는 델리게이트(int:남은 총알 개수)
    /// </summary>
    public Action<int> onBulletCountChange;

    /// <summary>
    /// 총알이 한발 발사되었음을 알리는 델리게이트(float:반동 정도)
    /// </summary>
    public Action<float> onFire;

    private void Awake()
    {
        muzzleEffect = GetComponentInChildren<VisualEffect>();
    }

    /// <summary>
    /// 초기화 용 함수
    /// </summary>
    void Initialize()
    {
        BulletCount = clipSize; // 총알을 완전히 장전된 상태로 초기화
        isFireReady = true;     // 발사 가능으로 초기화
    }

    /// <summary>
    /// 총을 발사하는 함수
    /// </summary>
    public void Fire(bool isFireStart = true)
    {
        if(isFireReady && BulletCount > 0)  // 발사 가능하고 총알이 남아있으면
        {
            FireProcess(isFireStart);       // 총 발사
        }
    }

    /// <summary>
    /// 발사가 성공했을 때 실행할 기능들
    /// </summary>
    /// <param name="isFireStart">발사 입력이 들어오면 true. 끝나면 false</param>
    protected virtual void FireProcess(bool isFireStart = true)
    {
        isFireReady = false;            // 계속 발사가 되지 않게 막기
        MuzzleEffectOn();               // 머즐 이팩트 보여주고
        BulletCount--;                  // 총알 개수 감소
        StartCoroutine(FireReady());    // 일정 시간 후에 자동으로 발사 가능하게 설정
    }

    /// <summary>
    /// 일정 시간후에 isFireReady를 true로 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FireReady()
    {
        yield return new WaitForSeconds(1/fireRate);    // fireRate에 따라 기다리는 시간 설정
        isFireReady = true;
    }

    /// <summary>
    /// Muzzle이팩트 실행하는 함수
    /// </summary>
    protected void MuzzleEffectOn()
    {
        muzzleEffect.SendEvent(onFireID);
    }

    /// <summary>
    /// 총이 부딪친곳에 따른 처리를 하는 함수
    /// </summary>
    protected void HitProcess()
    {
        Ray ray = new(fireTransform.position, GetFireDirection());  // 레이 만들기
        if( Physics.Raycast(ray, out RaycastHit hitInfo, range))    // 레이캐스트
        {
            Vector3 reflect = Vector3.Reflect(ray.direction, hitInfo.normal);
            Factory.Instance.GetBulletHole(hitInfo.point, hitInfo.normal, reflect); // 총알 구멍 생성을 위해, 생성될 위치, 생성될 면의 노멀, 반사방향 전달
        }
    }

    /// <summary>
    /// 반동을 알리는 함수
    /// </summary>
    protected void FireRecoil()
    {
        onFire?.Invoke(recoil);
    }

    /// <summary>
    /// 총을 장비할 때 처리를 수행하는 함수
    /// </summary>
    public void Equip()
    {
        fireTransform = GameManager.Instance.Player.FireTransform;
        Initialize();
    }

    /// <summary>
    /// 총이 장비 해제 될 떄 처리를 수행하는 함수
    /// </summary>
    public void UnEquip()
    {
        StopAllCoroutines();
        isFireReady = true;
    }

    /// <summary>
    /// 발사 각 안으로 랜덤한 발사 방향 구하는 함수
    /// </summary>
    /// <returns>총알을 발사할 방향</returns>
    protected Vector3 GetFireDirection()
    {
        Vector3 result = fireTransform.forward;

        // 위아래로 -spread ~ spread 만큼 회전(x축 기준으로 회전)
        result = Quaternion.AngleAxis(UnityEngine.Random.Range(-spread, spread), fireTransform.right) * result;

        // fireTransform.forward를 축으로 삼아 0 ~ 360도 회전
        result = Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), fireTransform.forward) * result;

        return result;
    }

#if UNITY_EDITOR

    public void Test_Fire(bool isFireStart = true)
    {
        if(fireTransform == null)
            Equip();
        Fire(isFireStart);
    }

    private void OnDrawGizmos()
    {
        if(fireTransform != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(fireTransform.position, fireTransform.position + fireTransform.forward * range);
        }
    }
#endif

}
