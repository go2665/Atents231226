using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

// 스프라이트만 계속 스왑하는 클래스
public class SawBlade : NetworkBehaviour
{
    // 애니메이션 스프라이트
    [SerializeField] private Sprite[] _sprites = new Sprite[2];

    protected SpriteRenderer _renderer;
    private bool _spriteController;
    
    // 타임 체크를 위한 float
    private float _spriteTimer;

    // _spriteTimer를 리셋하는 값
    private float _spriteChangeCD = .1f;

    public virtual void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _spriteTimer = _spriteChangeCD;     // 초기값 설정
    }

    void Update()
    {
        if (_spriteTimer > 0)   // _spriteTimer값이 남아있으면
        {
            _spriteTimer -= Time.deltaTime; // 계속 감소시키기
        }
        else
        {
            _spriteTimer = _spriteChangeCD; // 타이머가 다 됬으면 타이머 리셋
            ChangeSprite();                 // 스프라이트 변경
        }
    }

    // 스프라이트 스왑하는 함수
    private void ChangeSprite()
    {
        _renderer.sprite = _sprites[_spriteController ? 0 : 1];     // 0번째와 1번째를 계속 스왑
        _spriteController = !_spriteController;
    }
}
