using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class MovingSawBlade : SawBlade
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private List<Vector2> _positions = new List<Vector2>();
    [Networked]
    private float _delta { get; set; }
    [Networked]
    private int _posIndex { get; set; }
    [Networked]
    private Vector2 _currentPos { get; set; }
    [Networked]
    private Vector2 _desiredPos { get; set; }

    public override void Start()
    {
        base.Start();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Spawned()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _currentPos = transform.position;
        _desiredPos = _positions[0];
        _posIndex = 0;
    }

    public override void FixedUpdateNetwork()
    {
        transform.position = Vector2.Lerp(_currentPos, _desiredPos, _delta);
        _delta += Runner.DeltaTime * _speed;

        if (_delta >= 1)
        {
            _delta = 0;
            _currentPos = _positions[_posIndex];
            _posIndex = _posIndex < _positions.Count - 1 ? _posIndex + 1 : 0;
            _desiredPos = _positions[_posIndex];
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 lastPos = _positions[0];
        Gizmos.color = Color.red;
        foreach(Vector2 pos in _positions)
        {
            Gizmos.DrawLine(lastPos, pos);
            lastPos = pos;
        }
    }
}
