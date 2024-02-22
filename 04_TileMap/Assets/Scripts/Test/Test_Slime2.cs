using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime2 : TestBase
{
    public Renderer slimeRenderer;
    Material mainMaterial;

    private void Start()
    {
        mainMaterial = slimeRenderer.material;
    }

    void ResetShaderProperty()
    {
        //  - 리셋
    }

    void ShowOutline(bool isShow)
    {
        //  - Outline on/off
    }

    IEnumerator StartPhase()
    {
        //  - PhaseReverse로 안보이는 상태에서 보이게 만들기 (1->0)
        yield return null;
    }

    IEnumerator StartDissolve()
    {
        //  - Dissolve 실행시키기(1->0)
        yield return null;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        ResetShaderProperty();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        ShowOutline(true);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        ShowOutline(false);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        StartCoroutine(StartPhase());
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        StartCoroutine(StartDissolve());
    }


}
