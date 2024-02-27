using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Test_SlimePath : TestBase
{
    public Tilemap background;
    public Tilemap obstacle;
    public Slime slime;

    TileGridMap map;

    private void Start()
    {
        map = new TileGridMap(background, obstacle);
        slime.Initialize(map, new(1,1));
    }

    protected override void OnTestLClick(InputAction.CallbackContext _)
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2Int gridPos = map.WorldToGrid(worldPos);

        if(map.IsValidPosition(gridPos) && map.IsPlain(gridPos))
        {
            slime.SetDestination(gridPos);
        }
    }
}
