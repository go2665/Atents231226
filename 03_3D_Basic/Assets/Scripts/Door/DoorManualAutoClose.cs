using System.Collections;
using TMPro;
using UnityEngine;

public class DoorManualAutoClose : DoorManual, IInteracable
{
    public float autoCloseTime = 3.0f;

    public new void Use()   // 함수에 new 키워드가 붙으면 부모쪽의 함수를 무시한다.
    {
        Open();
        StopAllCoroutines();
        StartCoroutine(AutoClose());
    }

    IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(autoCloseTime);
        Close();
    }
}
