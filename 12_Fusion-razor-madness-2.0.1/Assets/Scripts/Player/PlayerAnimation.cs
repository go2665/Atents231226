using UnityEngine;

// 플레이어의 애니메이션 관리
public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _renderer;
    private PlayerRigidBodyMovement _movement;

    void Start()
    {
        // 컴포넌트 찾기
        _movement = GetComponentInParent<PlayerRigidBodyMovement>();
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (_movement.Velocity.x < -.1f)
        {
            _renderer.flipX = true;     // 왼쪽으로 움직이고 있으면 x플립 true
        }else if (_movement.Velocity.x > .1f)
        {
            _renderer.flipX = false;    // 오른쪽으로 움직이고 있으면 x플립 false
        }
        // 좌우로 안움직이고 있으면 이전 플립 상태 유지

        _anim.SetBool("Grounded", _movement.GetGrounded()); // 지표에 닫고 있는지를 에니메이터에게 전송
        _anim.SetFloat("Y_Speed", _movement.Velocity.y);    // 위아래 운동량 전달
        _anim.SetFloat("X_Speed_Abs", Mathf.Abs(_movement.Velocity.x)); // 좌우 운동량의 절대값 전달
    }
}
