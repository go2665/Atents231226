using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : RecycleObject
{    
    Animator animator;
    float animLength = 0.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // 애니메이터에서 클립 길이 받아오기
        // GetCurrentAnimatorClipInfo(0) : 애니메이터의 첫번째 레이어의 클립 정보들 받아오기
        // GetCurrentAnimatorClipInfo(0)[0] : 애니메이터의 첫번째 레이어에 있는 애니메이션 클립 중 첫번째 클립의 정보 받아오기 
        animLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;         
    }

    protected override void OnEnable()
    {
        //Time.timeScale = 0.1f;  // 시간 진행속도 1/10로 만들기        
        base.OnEnable();
        StartCoroutine(LifeOver(animLength));
    }


}
