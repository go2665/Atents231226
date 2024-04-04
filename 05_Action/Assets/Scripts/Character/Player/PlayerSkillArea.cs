using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    /// 스킬의 마나 소비량(초당)
    /// </summary>
    public float manaCost = 30.0f;

    /// <summary>
    /// 활성화 된 시점에서 스킬의 최종 공격력
    /// </summary>
    float finalPower;

    /// <summary>
    /// 활성화 상태를 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsActivate => gameObject.activeSelf;

    /// <summary>
    /// 트리거 안에 있는 적들의 목록
    /// </summary>
    List<Enemy> enemies = new List<Enemy>(4);

    /// <summary>
    /// 데미지 쿨 타임
    /// </summary>
    float coolTime = 0.0f;

    /// <summary>
    /// MP에 접근을 하기 위한 인터페이스
    /// </summary>
    IMana playerMana;

    private void Awake()
    {
        playerMana = GetComponentInParent<IMana>();
    }

    /// <summary>
    /// 스킬 영역 활성화
    /// </summary>
    /// <param name="power">플레이어의 현재 공격력</param>
    public void Activate(float power)
    {
        coolTime = 0.0f;                        // 쿨타임 초기화
        finalPower = power * (1 + skillPower);  // 최종 데미지 결정

        gameObject.SetActive(true);             // 오브젝트 활성화 (=트리거 켜기)
    }   

    /// <summary>
    /// 스킬 영역 비활성화
    /// </summary>
    public void Deactivate()
    {
        gameObject.SetActive(false);            // 오브젝트 비활성화
    }

    private void Update()
    {
        coolTime -= Time.deltaTime;
        if ( coolTime < 0 )
        {
            foreach (Enemy enemy in enemies )   // 트리거 안에 있는 모든 적에게 데미지 주기
            {
                enemy.Defence(finalPower);
            }
            coolTime = skillTick;               // 쿨타임 초기화
        }

        playerMana.MP -= manaCost * Time.deltaTime; // MP는 지속적으로 cost만큼 감소

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemies.Add(enemy); // 들어온 적 리스트에 추가
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Remove(enemy);  // 나간 적을 리스트에서 제거
            }
        }
    }
}
