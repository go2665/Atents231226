using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeploymentPanel : MonoBehaviour
{
    DeploymentToggle[] toggles;

    private void Awake()
    {
        toggles = GetComponentsInChildren<DeploymentToggle>();  // 모든 자식 토글버튼 찾기
        foreach (DeploymentToggle toggle in toggles)
        {
            toggle.onSelect += UnSelectOthers;      // 한 토글버튼이 눌려지면 다른 모든 토글버튼을 NotSelect상태로 만들기
        }
    }

    /// <summary>
    /// self를 제외한 형제 토글버튼을을 NotSelect로 만드는 함수
    /// </summary>
    /// <param name="self">지금 눌려진 토글 버튼(자기자신)</param>
    private void UnSelectOthers(DeploymentToggle self)
    {
        foreach(DeploymentToggle toggle in toggles)
        {
            if(toggle != self)
            {
                toggle.SetNotSelect();
            }
        }
    }
}
