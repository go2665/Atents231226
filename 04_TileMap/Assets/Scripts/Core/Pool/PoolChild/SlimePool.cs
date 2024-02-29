using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePool : ObjectPool<Slime>
{
    protected override void OnGenerateObject(Slime comp)
    {
        comp.Pool = comp.transform.parent;  // pool 설정
        comp.ShowPath(GameManager.Instance.showSlimePath);  // 경로 그릴지 말지 초기 설정
    }
}
