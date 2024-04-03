using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillArea : MonoBehaviour
{
    /// <summary>
    /// 스킬이 데미지를 주는 간격
    /// </summary>
    public float skillTick = 0.5f;

    /// <summary>
    /// 플레이어의 공격력을 증폭시키는 정도(0.2 = 20%)
    /// </summary>
    public float skillPower = 0.2f;

    /// <summary>
    /// 활성화 된 시점에서 스킬의 최종 공격력
    /// </summary>
    float finalPower;

    public void Activate(float power)
    {
        finalPower = power * (1 + skillPower);

        gameObject.SetActive(true);

        // 활성화되면 skillTick마다 트리거 안에 있는 모든 적에게 finalPower만큼의 데미지를 준다.
        // 칼의 이팩트는 켠다
        // 활성화 되어 있는 동안 지속적으로 플레이어의 MP가 감소한다.
        // 스킬 애니메이션 시작
    }

    public void Deactivate()
    {
        // 칼 이팩트 끄고 비활성화
        // 스킬 애니메이션 종료
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
