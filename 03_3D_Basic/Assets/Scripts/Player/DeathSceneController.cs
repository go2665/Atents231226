using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSceneController : MonoBehaviour
{
    CinemachineVirtualCamera vCam;
    CinemachineDollyCart cart;

    // 플레이어의 onDie 델리게이트에 함수를 연결

    // 이 오브젝트의 위치는 플레이어와 항상 같은 위치이어야 한다.
    // 자식으로 가상 카메라, 트랙, 카트가 있다.
    // 플레이어가 죽으면 카트가 움직이기 시작한다.
    // 플레이어가 죽으면 자식으로 가진 가상 카메라의 우선 순위가 가장 높아진다.
    // 가상 카메라는 항상 플레이어를 바라본다.
}
