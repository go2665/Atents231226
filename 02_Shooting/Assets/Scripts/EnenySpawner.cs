using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnenySpawner : MonoBehaviour
{
    // 실습
    // 1. 적을 스폰한다.
    // 2. 랜덤한 높이로 생성된다.(y : +4 ~ -4)
    // 

    private void Awake()
    {
        float rand = Random.Range(-4.0f, 4.0f);  // 랜덤으로 -4 ~ 4
    }
}
