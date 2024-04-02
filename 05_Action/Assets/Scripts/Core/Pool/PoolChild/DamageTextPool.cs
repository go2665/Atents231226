using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageTextPool : ObjectPool<DamageText>
{
    /// <summary>
    /// 풀에서 사용하지 않는 오브젝트를 하나 꺼낸 후 리턴 하는 함수
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    /// <param name="position">생성될 위치</param>
    /// <returns>풀에서 꺼낸 오브젝트(활성화됨)</returns>
    public GameObject GetObject(int damage, Vector3? position)
    {
        DamageText damageText = GetObject(position);
        damageText.SetDamage(damage);

        return damageText.gameObject;
    }
}
