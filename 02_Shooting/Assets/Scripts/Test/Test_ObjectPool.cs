using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ObjectPool : TestBase
{
    public BulletPool pool1;
    public WavePool pool2;
    public HitEffectPool pool3;
    public ExplosionEffectPool pool4;

#if UNITY_EDITOR
    private void Start()
    {
        //pool.Initialize();                  // 시작할 때 초기화
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Bullet bullet = pool1.GetObject();   // 풀에서 오브젝트 하나 꺼내기
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // hit
        pool3.GetObject();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // enemy
        pool2.GetObject();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // explosion
        pool4.GetObject();
    }
#endif
}
