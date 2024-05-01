using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSetter : MonoBehaviour
{
    /// <summary>
    /// 공격이 성공했을 때 보여질 표시
    /// </summary>
    public GameObject successPrefab;

    /// <summary>
    /// 공격이 실패했을 때 보여질 표시
    /// </summary>
    public GameObject failPrefab;

    /// <summary>
    /// 공격 받은 위치에 포탄 명중 여부를 표시해주는 함수
    /// </summary>
    /// <param name="world">공격 받은 위치(월드좌표, 그리드의 가운데)</param>
    /// <param name="isSuccess">공격이 성공했으면 true, 아니면 false</param>
    public void SetBombMark(Vector3 world, bool isSuccess)
    {
        GameObject prefab = isSuccess ? successPrefab : failPrefab; // 프리팹 결정
        GameObject inst = Instantiate(prefab, transform);           // 프리팹을 자식으로 생성

        world.y = transform.position.y;     // y는 보드 위치이어야 함
        inst.transform.position = world;    // 위치 설정
    }

    /// <summary>
    /// 모든 폭탄 표시를 제거하는 함수
    /// </summary>
    public void ResetBombMarks()
    {
        while(transform.childCount > 0)     // 자식이 남아있으면 계속 반복
        {
            Transform child = transform.GetChild(0);    // 첫번째 자식 선택
            child.SetParent(null);                      // 부모 제거(Destroy가 즉시 실행되지 않기 때문에 필요)
            Destroy(child.gameObject);                  // 자식 삭제
        }
    }
}
