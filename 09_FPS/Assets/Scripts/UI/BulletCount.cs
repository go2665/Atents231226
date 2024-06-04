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
        player.AddBulletCountChangeDelegate(OnBulletCountChange);
        player.onGunChange += OnGunChange;
    }

    void OnBulletCountChange(int count)
    {
        current.text = count.ToString();
    }

    void OnGunChange(GunBase gun)
    {
        max.text = gun.clipSize.ToString();
    }
}
