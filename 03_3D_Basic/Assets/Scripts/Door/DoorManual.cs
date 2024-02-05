using System.Collections;
using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    TextMeshPro text;   // 3D 글자(UI아님)
    
    /// <summary>
    /// 문이 열려있는 상태(true면 문이 열려있다, false면 문이 닫혀있다.)
    /// </summary>
    bool isOpen = false;

    /// <summary>
    /// 재사용 쿨타임
    /// </summary>
    public float coolTime = 0.5f;

    /// <summary>
    /// 현재 남아있는 쿨타임
    /// </summary>
    float currentCoolTime = 0;

    /// <summary>
    /// 사용 가능 여부. 쿨타임이 0 미만일 때 사용 가능
    /// </summary>
    public bool CanUse => currentCoolTime < 0.0f;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMeshPro>(true);
    }

    void Update()
    {
        currentCoolTime -= Time.deltaTime;
    }

    public void Use()
    {
        if(CanUse)  // 사용 가능할 때만 사용
        {
            if (isOpen)
            {
                Close();
                isOpen = false;
            }
            else
            {
                Open();
                isOpen = true;
            }
            currentCoolTime = coolTime; // 쿨타임 초기화
        }        
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 cameraToDoor = transform.position - Camera.main.transform.position;   // 카메라에서 문으로 향하는 방향 벡터

            float angle = Vector3.Angle(transform.forward, cameraToDoor);
            //Debug.Log(angle);
            if (angle > 90.0f)   // 사이각이 90도보다 크면 카메라가 문 앞에 있다.
            {
                text.transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0); // 문의 회전에서 y축으로 반바퀴 더 돌리기
            }
            else
            {
                text.transform.rotation = transform.rotation;   // 문의 회전 그대로 적용
            }

            text.gameObject.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }
}
