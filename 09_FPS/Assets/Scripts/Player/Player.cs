using StarterAssets;
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

    private void Awake()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<FirstPersonController>();

        gunCamera = transform.GetChild(2).gameObject;

        Transform child = transform.GetChild(3);
        guns = child.GetComponentsInChildren<GunBase>();    // 모든 총 찾기
        foreach(GunBase gun in guns)
        {
            gun.onFire += controller.FireRecoil;
        }
    }

    private void Start()
    {
        starterAssets.onZoom += DisableGunCamera;   // 줌 할 때 실행될 함수 연결
    }

    /// <summary>
    /// 총 표시하는 카메라 활성화 설정
    /// </summary>
    /// <param name="disable">true면 비활성화(총이 안보인다), false면 활성화(총이보인다)</param>
    void DisableGunCamera(bool disable = true)
    {
        gunCamera.SetActive(!disable);
    }
}
