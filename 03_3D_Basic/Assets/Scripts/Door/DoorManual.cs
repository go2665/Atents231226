using System.Collections;
using TMPro;
using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    TextMeshPro text;   // 3D 글자(UI아님)
    bool isOpen = false;

    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMeshPro>(true);
    }

    public void Use()
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
    }

    private void OnTriggerEnter(Collider other)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);
        }
    }
}
