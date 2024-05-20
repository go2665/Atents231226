using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    // 십자선이 원래 위치로 복구하는 속도를 지정하기 위한 커브 필요
    // 최대 확장 크기 지정하기
    // 마우스 좌클릭을 할 때마다 조금씩 확장. 시간이 지나면 원래 위치로 돌아옴

    /// <summary>
    /// 회복 속도를 조절하기 위한 커브
    /// </summary>
    public AnimationCurve recoveryCurve;

    /// <summary>
    /// 최대 확장 크기
    /// </summary>
    public float maxExpend = 100.0f;

    /// <summary>
    /// 기본적으로 확장되어 있는 길이(부모에서 기본적으로 떨어져 있는 정도)
    /// </summary>
    const float defaultExpend = 10.0f;

    /// <summary>
    /// 현재 확장되어 있는 길이(defaultExpend에서 얼마나 더 확장되었나)
    /// </summary>
    float current = 0.0f;

    /// <summary>
    /// 복구 되기 전에 기다리는 시간
    /// </summary>
    const float recoveryWaitTime = 0.1f;

    /// <summary>
    /// 복구 되는데 걸리는 시간
    /// </summary>
    const float recoveryDuration = 0.5f;

    /// <summary>
    /// 나누기를 자주하는 것을 피하기 위해 미리 계산해 놓은 것
    /// </summary>
    const float divPreCompute = 1 / recoveryDuration;

    /// <summary>
    /// 4방향 크로스해어 이미지의 트랜스폼들
    /// </summary>
    RectTransform[] crossRects;

    /// <summary>
    /// 각 크로스해어 이미지의 이동 방향
    /// </summary>
    readonly Vector2[] direction = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    private void Awake()
    {
        crossRects = new RectTransform[transform.childCount];       // 크로스해어의 이미지 미리 찾아 놓기
        for(int i=0; i<transform.childCount; i++)
        {
            crossRects[i] = transform.GetChild(i) as RectTransform; 
        }
    }

    /// <summary>
    /// 조준선을 확장시키는 함수
    /// </summary>
    /// <param name="amount">확장시키는 정도</param>
    public void Expend(float amount)
    {
        current = Mathf.Min(current + amount, maxExpend);   // 최대치를 넘지 않게 조절
        for(int i=0;i<crossRects.Length;i++)
        {
            crossRects[i].anchoredPosition = (defaultExpend + current) * direction[i];    // defaultExpend에서 current만큼 확장시키기
        }

        StopAllCoroutines();                                // 코루틴 중복실행을 방지하기 위해 실행되던 코루틴 정지
        StartCoroutine(DelayRecovery(recoveryWaitTime));    // defaultExpend로 복구 시키는 코루틴
    }

    /// <summary>
    /// defaultExpend로 복구 시키는 코루틴
    /// </summary>
    /// <param name="wait">처음에 대기하는 시간</param>
    /// <returns></returns>
    IEnumerator DelayRecovery(float wait)
    {
        yield return new WaitForSeconds(wait);  // 처음에 wait만큼 기다리기

        float startExpend = current;    // current를 이용해서 현재 확장 정도 기록해두기(최대치 설정)
        float curveProcess = 0.0f;      // 현재 커브 진행 정도(0 ~ 1)
        
        while(curveProcess < 1)         // curveProcess가 1이 될 때까지 계속 진행
        {
            curveProcess += Time.deltaTime * divPreCompute; // recoveryDuration 기간에 맞춰서 curveProcess 진행
            current = recoveryCurve.Evaluate(curveProcess) * startExpend;   // current를 계산하기(커브 결과 * 최대치)
            for (int i = 0; i < crossRects.Length; i++)
            {
                crossRects[i].anchoredPosition = (current + defaultExpend) * direction[i];  // 계산된 current에 맞게 축소
            }
            yield return null;          // 1프레임 대기
        }

        // deltaTime은 오차가 있을 수 밖에 없으니 깔끔하게 defaultExpend로 지정
        for (int i = 0; i < crossRects.Length; i++)
        {
            crossRects[i].anchoredPosition = defaultExpend * direction[i];
        }
        current = 0;                    // current 클리어
    }
}
