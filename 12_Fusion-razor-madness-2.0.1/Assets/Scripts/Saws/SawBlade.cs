using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SawBlade : NetworkBehaviour
{
    [SerializeField] private Sprite[] _sprites = new Sprite[2];
    protected SpriteRenderer _renderer;
    private bool _spriteController;
    private float _spriteTimer;
    private float _spriteChangeCD = .1f;

    public virtual void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _spriteTimer = _spriteChangeCD;
    }

    void Update()
    {
        if (_spriteTimer > 0)
        {
            _spriteTimer -= Time.deltaTime;
        }
        else
        {
            _spriteTimer = _spriteChangeCD;
            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        _renderer.sprite = _sprites[_spriteController ? 0 : 1];
        _spriteController = !_spriteController;
    }
}
