using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePanel : MonoBehaviour
{
    // 플레이어 생명 표시용 패널

    Image[] lifeImages;

    private void Start()
    {
        Player player = GameManager.Instance.Player;
        if(player != null)
        {
            player.onLifeChange += OnLifeChange;
        }
    }

    private void OnLifeChange(int life)
    {
        // 플레이어의 생명수치에 따라 표시 변경
        // 날아간 생명은 반투명한 회색으로 표시하기

        // 이미지 컴포넌트의 색상 변경용 프로퍼티
        //lifeImages[0].color 
    }
}
