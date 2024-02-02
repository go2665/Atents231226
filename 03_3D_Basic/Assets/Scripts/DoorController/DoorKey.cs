using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public float rotateSpeed = 360.0f;

    Transform modelTransform;

    public Action onConsume;

    private void Awake()
    {
        modelTransform = transform.GetChild(0);
    }

    private void Update()
    {
        modelTransform.Rotate(Time.deltaTime * rotateSpeed * Vector3.up);
    }


    //target = door.GetComponent<DoorAuto>();   // 권장
    //target = door as DoorAuto;  // door가 DoorAuto로 캐스팅 될 수 있으면 캐스팅하고 아니면 null

    // is : is 왼쪽에 있는 변수의 데이터 타입이, 오른쪽에 있는 타입이나 그 타입을 상속받은 타입이면, true 아니면 false
    // as : as 왼쪽에 있는 변수의 데이터 타입이, 오른쪽에 있는 타입이나 그 타입을 상속받은 타입이면,
    //      캐스팅을 해서 null이 아닌 값을 리턴하고, 아니면 null이다.

    private void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player"))
        {
            OnConsume();
        }
    }

    /// <summary>
    /// 이 열쇠를 획득했을 때 일어날 일을 처리하는 함수
    /// </summary>
    protected virtual void OnConsume()
    {
        onConsume?.Invoke();
        Destroy(this.gameObject);
    }

}
