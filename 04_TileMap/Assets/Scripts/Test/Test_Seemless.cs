using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Seemless : TestBase
{
    [Range(0, 2)]
    public int x = 0;

    [Range(0, 2)]
    public int y = 0;

    WorldManager world;

    private void Start()
    {
        world = GameManager.Instance.World;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        world.TestLoadScene(x, y);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        world.TestUnloadScene(x, y);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        world.TestRefreshscenes(x, y);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        world.TestUnloadAllScene();
    }

}
