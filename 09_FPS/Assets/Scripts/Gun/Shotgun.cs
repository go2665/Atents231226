using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : GunBase
{
    /// <summary>
    /// 한번에 발사되는 총알 개수
    /// </summary>
    public int pellet = 6;

    protected override void FireProcess(bool isFireStart = true)
    {
        if(isFireStart)
        {
            base.FireProcess(isFireStart);

            for(int i=0; i < pellet; i++)
            {
                HitProcess();   // 여러번 Hit 처리
            }

            FireRecoil();
        }
    }
}
