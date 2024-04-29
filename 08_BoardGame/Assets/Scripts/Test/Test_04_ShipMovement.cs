using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_04_ShipMovement : TestBase
{
    public Board board;
    public Ship ship;

    protected override void OnEnable()
    {
        base.OnEnable();
        inputActions.Test.MouseMove.performed += OnMouseMove;
        inputActions.Test.MouseWheel.performed += OnMouseWheel;
    }

    protected override void OnDisable()
    {
        inputActions.Test.MouseWheel.performed -= OnMouseWheel;
        inputActions.Test.MouseMove.performed -= OnMouseMove;
        base.OnDisable();
    }

    private void Start()
    {
        ship.Initialize(ShipType.Carrier);
        ship.gameObject.SetActive(true);
    }

    private void OnMouseMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(ship != null)
        {
            //Debug.Log(context.ReadValue<Vector2>());
            Vector2Int grid = board.GetMouseGridPosition();
            Vector3 world = board.GridToWorld(grid);
            ship.transform.position = world;
        }
    }

    private void OnMouseWheel(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(ship != null)
        {
            //Debug.Log(context.ReadValue<float>());
            float wheel = context.ReadValue<float>();
            if (wheel > 0)
                ship.Rotate(false);
            else
                ship.Rotate(true);
        }
    }

}