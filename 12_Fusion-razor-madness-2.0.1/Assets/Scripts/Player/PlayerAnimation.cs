using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _renderer;
    private PlayerRigidBodyMovement _movement;

    void Start()
    {
        _movement = GetComponentInParent<PlayerRigidBodyMovement>();
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (_movement.Velocity.x < -.1f)
        {
            _renderer.flipX = true;
        }else if (_movement.Velocity.x > .1f)
        {
            _renderer.flipX = false;
        }

        _anim.SetBool("Grounded", _movement.GetGrounded());
        _anim.SetFloat("Y_Speed", _movement.Velocity.y);
        _anim.SetFloat("X_Speed_Abs", Mathf.Abs(_movement.Velocity.x));
    }
}
