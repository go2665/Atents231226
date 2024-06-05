using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletCount : MonoBehaviour
{
    // 완성시키기
    // 1. 총을 쏠 때나마 남은 총알 개수 갱신하기
    // 2. 총을 장비할 때마다 최대 총알 개수 표시하기

    TextMeshProUGUI current;
    TextMeshProUGUI max;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        current = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        max = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        player.AddAmmoCountChangeDelegate(OnAmmoCountChange);

        player.onGunChange += OnGunChange;
    }

    /// <summary>
    /// 총알 개수 변경시 실행되는 함수
    /// </summary>
    /// <param name="count"></param>
    void OnAmmoCountChange(int count)
    {
        current.text = count.ToString();
    }

    /// <summary>
    /// 총이 변경될 때 실행되는 함수
    /// </summary>
    /// <param name="gun"></param>
    void OnGunChange(GunBase gun)
    {
        max.text = gun.clipSize.ToString();
    }
}
