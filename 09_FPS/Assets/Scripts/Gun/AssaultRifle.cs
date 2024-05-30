using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : GunBase
{
    protected override void FireProcess(bool isFireStart = true)
    {
        if(isFireStart) 
        {
            // 입력이 들어왔을때 발사 시작
            StartCoroutine(FireRepeat());
        }
        else
        {
            // 입력이 끝났을 때 발사 종료
            StopAllCoroutines();
            isFireReady = true;
        }
    }

    IEnumerator FireRepeat()
    {
        while(BulletCount > 0)  // 총알이 남아있는 동안 계속 반복
        {
            MuzzleEffectOn();   // 머즐 이팩트 켜고
            BulletCount--;      // 총알 개수 하나 줄이기

            HitProcess();       // 명중 처리

            FireRecoil();       // 반동 주기

            yield return new WaitForSeconds(1 / fireRate);  // 발사 속도 만큼 대기
        }
        isFireReady = true;
    }
}
