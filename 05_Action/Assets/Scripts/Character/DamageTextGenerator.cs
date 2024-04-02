using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextGenerator : MonoBehaviour
{
    private void Start()
    {
        IBattler battler = GetComponentInParent<IBattler>();
        battler.onHit += DamageTextGenerate;
    }

    void DamageTextGenerate(int damage)
    {
        Factory.Instance.GetDamageText(damage, transform.position);
    }
}
