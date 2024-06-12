using StarterAssets;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 유니티가 제공하는 시작용 코드(입력처리용 함수를 모아 놓은 클래스)
    /// </summary>
    StarterAssetsInputs starterAssets;

    /// <summary>
    /// 유니티가 제공하는 입력 처리용 코드
    /// </summary>
    FirstPersonController controller;

    /// <summary>
    /// 총만 촬영하는 카메라가 있는 게임 오브젝트
    /// </summary>
    GameObject gunCamera;

    /// <summary>
    /// 총은 카메라 기준으로 발사됨
    /// </summary>
    public Transform FireTransform => transform.GetChild(0);    // 카메라 루트

    /// <summary>
    /// 플레이어가 장비할 수 있는 모든 총
    /// </summary>
    GunBase[] guns;

    /// <summary>
    /// 현재 장비하고 있는 총
    /// </summary>
    GunBase activeGun;

    /// <summary>
    /// 최대 HP
    /// </summary>
    public float MaxHP = 100.0f;

    /// <summary>
    /// 현재 HP
    /// </summary>
    float hp;

    /// <summary>
    /// 현재 HP 확인 및 설정용 프로퍼티
    /// </summary>
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(hp <= 0)
            {
                Die();  // HP가 0 이하면 사망
            }
            hp = Mathf.Clamp(hp, 0, MaxHP); // HP 최대 최소 안벗어나게 만들기
            onHPChange?.Invoke(hp);         // HP 변화 알리기
        }
    }


    /// <summary>
    /// 총이 변경되었음을 알리는 델리게이트
    /// </summary>
    public Action<GunBase> onGunChange;

    /// <summary>
    /// 공격을 받았을 때 실행될 델리게이트(float:공격 받은 각도. 플레이어 forward와 적으로 가는 방향 벡터 사이의 각도. 시계방향)
    /// </summary>
    Action<float> onAttacked;

    /// <summary>
    /// HP가 변경되었을 때 실행될 델리게이트(float:현재 HP)
    /// </summary>
    Action<float> onHPChange;

    /// <summary>
    /// 플레이어가 죽었을 때 실행될 델리게이트
    /// </summary>
    public Action onDie;

    private void Awake()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<FirstPersonController>();

        gunCamera = transform.GetChild(2).gameObject;

        Transform child = transform.GetChild(3);
        guns = child.GetComponentsInChildren<GunBase>(true);    // 모든 총 찾기        
    }

    private void Start()
    {
        starterAssets.onZoom += DisableGunCamera;   // 줌 할 때 실행될 함수 연결

        Crosshair crosshair = FindAnyObjectByType<Crosshair>();
        foreach (GunBase gun in guns)
        {
            gun.onFire += controller.FireRecoil;                        // 화면 튕기는 효과
            gun.onFire += (expend) => crosshair.Expend(expend * 10);    // 조준선 확장 효과
            gun.onAmmoDepleted += () =>
            {
                if (!(activeGun is Revolver))
                {
                    GunChange(GunType.Revolver);    // 총알이 다 떨어지면 기본총으로 변경
                }
            };
        }
        activeGun = guns[0];    // 기본총 설정
        activeGun.Equip();      // 기본총 장비
        onGunChange?.Invoke(activeGun); // 총 변경 알림
    }

    /// <summary>
    /// 총 표시하는 카메라 활성화 설정
    /// </summary>
    /// <param name="disable">true면 비활성화(총이 안보인다), false면 활성화(총이보인다)</param>
    void DisableGunCamera(bool disable = true)
    {
        gunCamera.SetActive(!disable);
    }

    /// <summary>
    /// 장비중인 총을 변경하는 함수
    /// </summary>
    /// <param name="gunType">총의 종류</param>
    public void GunChange(GunType gunType)
    {
        activeGun.UnEquip();
        activeGun.gameObject.SetActive(false);  // 이전 총 비활성화하고 장비 해제하기

        activeGun = guns[(int)gunType];         // 새총 설정하고 장비하고 활성화하기
        activeGun.Equip();
        activeGun.gameObject.SetActive(true);

        onGunChange?.Invoke(activeGun);         // 총이 변경되었음을 알림
    }

    /// <summary>
    /// 장비중인 총을 발사하는 함수
    /// </summary>
    /// <param name="isFireStart">true면 발사버튼을 눌렀다, false면 발사버튼을 땠다.</param>
    public void GunFire(bool isFireStart)
    {
        activeGun.Fire(isFireStart);
    }

    /// <summary>
    /// 리볼버를 재장전하는 함수
    /// </summary>
    public void RevolverReload()
    {
        Revolver revolver = activeGun as Revolver;
        if(revolver != null)    // activeGun이 리볼버일 때만 재장전
        {
            revolver.Reload();
        }
    }

    /// <summary>
    /// 총알 개수가 변경될 때 실행되는 델리게이트에 콜백함수 추가
    /// </summary>
    /// <param name="callback">추가할 콜백함수</param>
    public void AddAmmoCountChangeDelegate(Action<int> callback)
    {
        foreach(var gun in guns)
        {
            gun.onAmmoCountChange += callback;
        }
    }

    /// <summary>
    /// 공격을 받았을 때 실행되는 함수
    /// </summary>
    /// <param name="enemy">공격을 한 적</param>
    public void OnAttacked(Enemy enemy)
    {
        float angle = 0.0f;         // 공격당한 각도(시계방향)
        onAttacked?.Invoke(angle);
        HP -= enemy.attackPower;
    }

    /// <summary>
    /// 플레이어 사망 처리용 함수
    /// </summary>
    private void Die()
    {
        onDie?.Invoke();                // 죽었음을 알림
        gameObject.SetActive(false);    // 플레이어 오브젝트 비활성화
    }
}
