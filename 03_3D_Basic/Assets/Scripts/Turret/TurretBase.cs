using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TurretBase : MonoBehaviour
{
    /// <summary>
    /// 터렛이 발사할 총알 종류
    /// </summary>
    public PoolObjectType bulletType = PoolObjectType.Bullet;

    /// <summary>
    /// 총알 발사 간격
    /// </summary>
    public float fireInterval = 1.0f;

    /// <summary>
    /// 총알 발사 위치 설정용 트랜스폼
    /// </summary>
    protected Transform fireTransform;

    /// <summary>
    /// 총몸의 트랜스폼
    /// </summary>
    protected Transform barrelBody;

    /// <summary>
    /// 총알을 주기적으로 발사하는 코루틴
    /// </summary>
    protected IEnumerator fireCoroutine;


    protected virtual void Awake()
    {
        barrelBody = transform.GetChild(2);
        fireTransform = barrelBody.GetChild(1);
        fireCoroutine = PeriodFire();
    }

    /// <summary>
    /// 인터벌 당 한번씩 총알을 쏘는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator PeriodFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireInterval);
            Factory.Instance.GetObject(bulletType, fireTransform.position, fireTransform.rotation.eulerAngles);
        }
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Vector3 p0 = transform.position;
        Vector3 p1 = transform.position + transform.forward * 2.0f;
        Vector3 p2 = p1 + Quaternion.AngleAxis(25.0f, transform.up) * (-transform.forward * 0.2f);
        Vector3 p3 = p1 + Quaternion.AngleAxis(-25.0f, transform.up) * (-transform.forward * 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p1, p3);
    }
#endif
}
