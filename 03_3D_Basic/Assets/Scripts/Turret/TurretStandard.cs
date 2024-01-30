using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStandard : MonoBehaviour
{
    public PoolObjectType bulletType = PoolObjectType.Bullet;

    public float fireInterval = 1.0f;

    Transform fireTransform;

    private void Awake()
    {
        Transform child = transform.GetChild(2);
        fireTransform = child.GetChild(1);
    }

    private void Start()
    {
        StartCoroutine(PeriodFire());
    }

    IEnumerator PeriodFire()
    {
        while (true)
        {
            Factory.Instance.GetObject(bulletType, fireTransform.position, fireTransform.rotation.eulerAngles);
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
