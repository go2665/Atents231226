using UnityEngine;

public interface IConsumable
{
    /// <summary>
    /// 아이템을 소비시키는 함수
    /// </summary>
    /// <param name="target">아이템에 영향을 받을 대상</param>
    void Consume(GameObject target);
}