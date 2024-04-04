using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ImageNumber : TestBase
{
    [Range(-99,999)]
    public int testNumber = 0;

    ImageNumber imageNumber;

    private void Start()
    {
        imageNumber = FindAnyObjectByType<ImageNumber>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        imageNumber.Number = testNumber;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        imageNumber.Number = Random.Range(-99,999);
    }
}
