using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime : TestBase
{
    /// <summary>
    /// 슬라임들의 랜더러(0:아웃라인, 1:페이즈, 2:리버스페이즈)
    /// </summary>
    public Renderer[] slimes;

    /// <summary>
    /// 슬라임들의 머티리얼들(0:아웃라인, 1:페이즈, 2:리버스페이즈)
    /// </summary>
    Material[] materials;

    /// <summary>
    /// 쉐이더 프로퍼티 변경 속도
    /// </summary>
    public float speed = 1.0f;

    // 쉐이더 프로퍼티 변경 on/off용 변수
    public bool outlineThicknessChange = false;
    public bool phaseSplitChange = false;
    public bool phaseThicknessChange = false;

    /// <summary>
    /// 시간 누적용(삼각함수에서 사용)
    /// </summary>
    float timeElapsed = 0.0f;

    /// <summary>
    /// split 정도(페이즈, 페이즈리버스)
    /// </summary>
    [Range(0f, 1f)]
    public float split = 0.0f;

    /// <summary>
    /// 페이즈류의 띠 두깨
    /// </summary>
    [Range(0.1f, 0.2f)]
    public float phaseThickness = 0.01f;

    /// <summary>
    /// 아웃라인의 두깨
    /// </summary>
    [Range(0.0f, 0.01f)]
    public float outlineThickness = 0.005f;

    // 프로퍼티 ID를 숫자로 미리 변경
    readonly int SplitID = Shader.PropertyToID("_Split");
    readonly int ReverseSplitID = Shader.PropertyToID("_ReverseSplit");
    readonly int OutlineThicknessID = Shader.PropertyToID("_Thickness");
    readonly int PhaseThicknessID = Shader.PropertyToID("_PhaseThickness");
    readonly int ReverseThicknessID = Shader.PropertyToID("_ReverseThickness");
        

    private void Start()
    {
        materials = new Material[slimes.Length];    // 머티리얼 미리 찾아서 저장
        for (int i=0;i<materials.Length;i++)
        {
            materials[i] = slimes[i].material;
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        float num = (Mathf.Cos(timeElapsed * speed) + 1.0f) * 0.5f; // 시간 변화에 따라 num값이 0 ~ 1로 계속 핑퐁된다.

        if( outlineThicknessChange )
        {
            float min = 0.0f;
            float max = 0.01f;
            num = min + (max - min) * num;      // num값에 따라 최소~최대로 변경

            // min = 5;
            // max = 10;
            // num이 0이면 5
            // num이 0.5이면 7.5
            // num이 1이면 10

            materials[0].SetFloat(OutlineThicknessID, num);
            outlineThickness = num;
        }
        if( phaseSplitChange )
        {
            materials[1].SetFloat(SplitID, num);
            materials[2].SetFloat(ReverseSplitID, num);
            split = num;
        }
        if( phaseThicknessChange )
        {
            float min = 0.1f;
            float max = 0.2f;
            num = min + (max - min) * num;

            materials[1].SetFloat(PhaseThicknessID, num);
            materials[2].SetFloat(ReverseThicknessID, num);
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Renderer renderer = slime.GetComponent<Renderer>();
        //Material material = renderer.material;
        //int id = Shader.PropertyToID("_Split");
        ////material.SetFloat("_Split", split);
        //material.SetFloat(id, split);

        // 아웃라인의 두깨 변경해보기
        outlineThicknessChange = !outlineThicknessChange;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // Phase와 ReversePhase split 변경하기
        phaseSplitChange = !phaseSplitChange;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // Phase와 ReversePhase의 두깨 변경하기
        phaseThicknessChange = !phaseThicknessChange;
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // InnerLine 두깨 조정하기
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        // Dissolve의 fade 조정하기
    }
}
