using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

// 움직이는 톱
public class MovingSawBlade : SawBlade
{
    // 이동 속도
    [SerializeField] private float _speed = 1;

    // 움직일 위치의 리스트(월드좌표)
    [SerializeField] private List<Vector2> _positions = new List<Vector2>();
    
    // 목적지까지 움직인 정도(0~1)
    [Networked]
    private float _delta { get; set; }

    // 목적지의 인덱스
    [Networked]
    private int _posIndex { get; set; }

    // 이전 인덱스의 위치(목적지에 도착했을 때만 설정됨)
    [Networked]
    private Vector2 _currentPos { get; set; }

    // 현재 목적지
    [Networked]
    private Vector2 _desiredPos { get; set; }

    public override void Start()
    {
        base.Start();
        // 부모와 다른 위치에 있는 컴포넌트 찾기
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Spawned()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();   // 스폰 이후에 다시 찾기
        _currentPos = transform.position;   // 현재 위치 저장
        _desiredPos = _positions[0];        // 목표지점은 리스트의 첫번째로 지정
        _posIndex = 0;                      // 인덱스도 0으로 설정
    }

    public override void FixedUpdateNetwork()
    {
        transform.position = Vector2.Lerp(_currentPos, _desiredPos, _delta);    // 보간으로 새 위치 결정
        _delta += Runner.DeltaTime * _speed;    // _delta는 계속 증가시킴

        if (_delta >= 1)    // 도착했으면
        {
            _delta = 0;     // 델타 초기화
            _currentPos = _positions[_posIndex];    // 현재 위치를 이전 목표지점으로 설정
            _posIndex = _posIndex < _positions.Count - 1 ? _posIndex + 1 : 0;   // 다음 인덱스 설정
            _desiredPos = _positions[_posIndex];    // 다음 목표지점 결정
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 lastPos = _positions[0];    // 마지막으로 그렸던 위치
        Gizmos.color = Color.red;
        foreach(Vector2 pos in _positions)  // 리스트 순회하면서 선 그리기
        {
            Gizmos.DrawLine(lastPos, pos);
            lastPos = pos;  // 마지막으로 그렸던 위치 갱신
        }
    }
}
