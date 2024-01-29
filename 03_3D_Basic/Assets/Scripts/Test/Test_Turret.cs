using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Turret : TestBase
{
    public GameObject bulletPrefab;
    public float interval = 0.1f;

    Transform fireTransform;

    private void Start()
    {
        fireTransform = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(bulletPrefab, fireTransform);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        StartCoroutine(FireCountinuos());
    }

    IEnumerator FireCountinuos()
    {
        while (true)
        {
            Instantiate(bulletPrefab, fireTransform);
            yield return new WaitForSeconds(interval);
        }
    }
}
