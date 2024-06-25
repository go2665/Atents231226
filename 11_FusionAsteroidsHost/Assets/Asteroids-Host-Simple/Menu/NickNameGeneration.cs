using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Asteroids.HostSimple
{
    // 메뉴씬의 플레이어 이름용 플레이스 홀더에 랜덤한 이름 설정하는 클래스
    public class NickNameGeneration : MonoBehaviour
    {
        private void Awake()
        {
            var nickNameInputField = GetComponentInChildren<TextMeshProUGUI>();
            nickNameInputField.text = PlayerData.GetRandomNickName();
        }
    }
}