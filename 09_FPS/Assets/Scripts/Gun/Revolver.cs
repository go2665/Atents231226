using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : GunBase
{
    public float reloadDuration = 1.0f;

    protected override void FireProcess()
    {
        base.FireProcess();
        HitProcess();       // 명중 처리하기
        FireRecoil();       // 총기 반동 신호 보내기
    }

    public void Reload()
    {

    }
}
