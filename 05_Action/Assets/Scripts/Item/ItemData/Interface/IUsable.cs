using UnityEngine;

public interface IUsable
{
    /// <summary>
    /// 아이템을 사용하는 함수
    /// </summary>
    /// <param name="target">아이템 효과를 받을 대상</param>
    /// <returns>사용 성공여부</returns>
    bool Use(GameObject target);
}