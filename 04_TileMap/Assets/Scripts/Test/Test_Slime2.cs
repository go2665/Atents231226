using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime2 : TestBase
{
    /// <summary>
    /// 코드로 이펙트를 조정할 머티리얼을 가진 랜더러
    /// </summary>
    public Renderer slimeRenderer;

    /// <summary>
    /// 코드로 조정할 머티리얼
    /// </summary>
    Material mainMaterial;

    /// <summary>
    /// 아웃라인이 보일 때의 두깨
    /// </summary>
    const float VisibleOutlineThickness = 0.004f;

    /// <summary>
    /// 페이즈가 보일 때의 두깨
    /// </summary>
    const float VisiblePhaseThickness = 0.1f;

    // 쉐이더 프로퍼티 아이디들
    readonly int OutlineThicknessID = Shader.PropertyToID("_OutlineThickness");
    readonly int PhaseSplitID = Shader.PropertyToID("_PhaseSplit");
    readonly int PhaseThicknessID = Shader.PropertyToID("_PhaseThickness");
    readonly int DissolveFadeID = Shader.PropertyToID("_DissolveFade");

    private void Start()
    {
        mainMaterial = slimeRenderer.material;  // 머티리얼 가져오기
    }

    void ResetShaderProperty()
    {
        //  - 리셋
        ShowOutline(false);                         // 아웃라인 끄고
        mainMaterial.SetFloat(PhaseThicknessID, 0); // 페이즈 선 안보이게 하기
        mainMaterial.SetFloat(PhaseSplitID, 0);     // 전신 보이게 하기
        mainMaterial.SetFloat(DissolveFadeID, 1);   // 디졸브 안보이게 하기
    }

    /// <summary>
    /// 아웃라인 켜고 끄는 함수
    /// </summary>
    /// <param name="isShow">true면 보이고 false면 보이지 않는다.</param>
    void ShowOutline(bool isShow)
    {
        //  - Outline on/off
        if(isShow)
        {
            mainMaterial.SetFloat(OutlineThicknessID, VisibleOutlineThickness); // 보이는 것은 두께를 설정하는 것으로 보이게 만듬
        }
        else
        {
            mainMaterial.SetFloat(OutlineThicknessID, 0);   // 안보이는 것은 두께를 0으로 만들어서 안보이게 만듬
        }
    }

    /// <summary>
    /// 페이즈 진행하는 코루틴(안보기->보이기)
    /// </summary>
    /// <returns></returns>
    IEnumerator StartPhase()
    {
        //  - PhaseReverse로 안보이는 상태에서 보이게 만들기 (1->0)

        float phaseDuration = 0.5f;                     // 페이즈 진행시간
        float phaseNormalize = 1.0f / phaseDuration;    // 나누기 계산을 줄이기 위해 미리 계산

        float timeElapsed = 0.0f;   // 시간 누적용

        mainMaterial.SetFloat(PhaseThicknessID, VisiblePhaseThickness); // 페이즈 선을 보이게 만들기

        while(timeElapsed < phaseDuration)  // 시간진행에 따라 처리
        {
            timeElapsed += Time.deltaTime;  // 시간 누적

            //mainMaterial.SetFloat(PhaseSplitID,  1 - (timeElapsed / dissolveDuration));
            mainMaterial.SetFloat(PhaseSplitID,  1 - (timeElapsed * phaseNormalize));   // split 값을 누적한 시간에 따라 변경

            yield return null;
        }

        mainMaterial.SetFloat(PhaseThicknessID, 0); // 페이즈 선 안보이게 만들기
        mainMaterial.SetFloat(PhaseSplitID, 0);     // 숫자를 깔끔하게 정리하기 위한 것
    }

    /// <summary>
    /// 디졸브 진행하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDissolve()
    {
        //  - Dissolve 실행시키기(1->0)
        float dissolveDuration = 0.5f;
        float dissolveNormalize = 1.0f / dissolveDuration;

        float timeElapsed = 0.0f;

        while (timeElapsed < dissolveDuration)
        {
            timeElapsed += Time.deltaTime;

            //mainMaterial.SetFloat(PhaseSplitID,  1 - (timeElapsed / dissolveDuration));
            mainMaterial.SetFloat(DissolveFadeID, 1 - (timeElapsed * dissolveNormalize));

            yield return null;
        }
        mainMaterial.SetFloat(DissolveFadeID, 0);
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
