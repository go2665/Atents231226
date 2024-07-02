using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RotatingSaw : SawBlade
{
    // 속도?
    [SerializeField] private int _speed = 1;

    // 회전 반지름?
    [SerializeField] private float _radius = 1;

    // 시작 각도
    [SerializeField] private int _startAngle = 0;

    // 랜덤 각도?
    [SerializeField] private bool _randomStartAngle = true;

    // 라인랜더러(쇠사슬?)
    private LineRenderer _line;

    // 회전 중심점?
    [Networked]
    private Vector2 Origin { get; set; }
    
    // 현재 각도
    [Networked]
    private int Index { get; set; }

    // 값 변경 감지용
    private ChangeDetector _changeDetector;

    public override void Start()
    {
        base.Start();
        _renderer = GetComponentInChildren<SpriteRenderer>();   // 위치가 변경되었으므로 다시 찾기
    }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState, false);  // 값 변경 감지 시작
        _line = GetComponent<LineRenderer>();                   // 라인 랜더러 찾기
        _renderer = GetComponentInChildren<SpriteRenderer>();   // 랜더러 찾기
        Origin = transform.position;                            // 자기 위치를 원점으로 설정
        Index = _randomStartAngle ? Random.Range(0, 360) : _startAngle; // 현재 각도 설정
        SetupLineRender();
    }

    // 라인 랜더러 초기화
    private void SetupLineRender()
    {
        _line.positionCount = 2;        // 점 2개로 이루어진 선
        _line.SetPosition(0, Origin);   // 중심점을 라인랜더러의 시작점으로 설정
    }

    public override void FixedUpdateNetwork()
    {
        transform.position = PointOnCircle(_radius, Index, Origin); // 원의 표면 중 한 위치를 구하기
        Index = Index >= 360 ? 0 : Index + (1 * _speed);            // index는 0~360를 계속 반복
    }

    public override void Render()
    {
        _line.SetPosition(1, transform.position);   // 톱의 현재 위치를 라인랜더러의 도착점으로 설정

        foreach (var changes in _changeDetector.DetectChanges(this))
        {
            switch (changes)
            {
                case nameof(Origin):
                    SetupLineRender();  // 톱의 위치가 변경되면 라인랜더러 다시 초기화
                    break;
            }
        }
    }

    // 원 표면의 위치를 구하는 함수
    public static Vector2 PointOnCircle(float radius, float angleInDegrees, Vector2 origin)
    {
        // Convert from degrees to radians via multiplication by PI/180        
        float x = (float)(radius * Mathf.Cos(angleInDegrees * Mathf.PI / 180f)) + origin.x;
        float y = (float)(radius * Mathf.Sin(angleInDegrees * Mathf.PI / 180f)) + origin.y;

        return new Vector2(x, y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 last = PointOnCircle(_radius, 0, transform.position);   // 톱이 움직이는 궤도 보여주기
        for (int i = 1; i < 360; i++)
        {
            Gizmos.DrawLine(last, PointOnCircle(_radius, i, transform.position));
            last = PointOnCircle(_radius, i, transform.position);
        }
    }
}
