using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RotatingSaw : SawBlade
{
    [SerializeField] private int _speed = 1;
    [SerializeField] private float _radius = 1;
    [SerializeField] private int _startAngle = 0;
    [SerializeField] private bool _randomStartAngle = true;

    private LineRenderer _line;
    [Networked]
    private Vector2 Origin { get; set; }
    [Networked]
    private int Index { get; set; }

    private ChangeDetector _changeDetector;

    public override void Start()
    {
        base.Start();
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState, false);
        _line = GetComponent<LineRenderer>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        Origin = transform.position;
        Index = _randomStartAngle ? Random.Range(0, 360) : _startAngle;
        SetupLineRender();
    }

    private void SetupLineRender()
    {
        _line.positionCount = 2;
        _line.SetPosition(0, Origin);
    }

    public override void FixedUpdateNetwork()
    {
        transform.position = PointOnCircle(_radius, Index, Origin);
        Index = Index >= 360 ? 0 : Index + (1 * _speed);
    }

    public override void Render()
    {
        _line.SetPosition(1, transform.position);

        foreach (var changes in _changeDetector.DetectChanges(this))
        {
            switch (changes)
            {
                case nameof(Origin):
                    SetupLineRender();
                    break;
            }
        }
    }

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
        Vector2 last = PointOnCircle(_radius, 0, transform.position);
        for (int i = 1; i < 360; i++)
        {
            Gizmos.DrawLine(last, PointOnCircle(_radius, i, transform.position));
            last = PointOnCircle(_radius, i, transform.position);
        }
    }
}
