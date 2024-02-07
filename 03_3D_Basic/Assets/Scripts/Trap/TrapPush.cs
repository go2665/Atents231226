using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPush : TrapBase
{
    public float pushPower = 5.0f;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnTrapActivate(GameObject target)
    {
        animator.SetTrigger("Activate");
        Rigidbody rigid = target.GetComponent<Rigidbody>();
        if(rigid != null)
        {
            Vector3 dir = (transform.up + transform.forward).normalized;
            rigid.AddForce(pushPower * dir, ForceMode.Impulse);
        }
    }
}
