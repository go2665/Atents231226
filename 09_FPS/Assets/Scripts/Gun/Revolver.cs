using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    public float reloadDuration = 1.0f;
    bool isReloading = false;

    protected override void FireProcess(bool isFireStart = true)
    {
        if(isFireStart)
        {
            base.FireProcess();
            HitProcess();       // 명중 처리하기
            FireRecoil();       // 총기 반동 신호 보내기
        }
    }

    /// <summary>
    /// 리로드 처리하는 함수
    /// </summary>
    public void Reload()
    {
        if(!isReloading)                        // 리로딩 중이 아닐 때만 실행
        {
            isReloading = true;                 // 리로딩 중이라고 표시
            isFireReady = false;                // 리로딩 중 총을 발사하는 것 방지
            StartCoroutine(ReloadCoroutine());  // 리로드 코루틴 실행
        }
    }

    /// <summary>
    /// 리로딩하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(reloadDuration);    // 리로딩 시간만큼 기다린 후
        isFireReady = true;         // 총 발사 가능하게 설정
        BulletCount = clipSize;     // 탄창 크기만큼 재장전
        isReloading = false;        // 리로딩 끝났다고 표시
    }
}
