using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    public float reloadDuration = 1.0f;

    protected override void FireProcess(bool isFireStart = true)
    {
        base.FireProcess();
        HitProcess();       // 명중 처리하기
        FireRecoil();       // 총기 반동 신호 보내기
    }

    public void Reload()
    {
        // 현재 총알 개수가 탄창크기만큼 설정된다.
        // 재장전 중에 다시 재장전하는 것은 안된다.
    }
}
