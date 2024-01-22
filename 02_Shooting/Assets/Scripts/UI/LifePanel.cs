using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifePanel : MonoBehaviour
{
    // 플레이어 생명 표시용 패널

    Image[] lifeImages;

    public Color disableColor;

    private void Awake()
    {
        lifeImages = new Image[transform.childCount];
        for (int i=0;i<transform.childCount;i++)
        {
            Transform child = transform.GetChild(i);
            lifeImages[i] = child.GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        Player player = GameManager.Instance.Player;
        if(player != null)
        {
            player.onLifeChange += OnLifeChange;
        }
    }

    private void OnDisable()
    {
        if(GameManager.Instance != null )
        {
            Player player = GameManager.Instance.Player;
            player.onLifeChange -= OnLifeChange;
        }
    }

    private void OnLifeChange(int life)
    {
        // 플레이어의 생명수치에 따라 표시 변경
        for(int i=0;i<life;i++)
        {
            lifeImages[i].color = Color.white;
        }
        for(int i=life;i<lifeImages.Length;i++)
        {
            lifeImages[i].color = disableColor;         // 날아간 생명은 반투명한 회색으로 표시하기
        }

    }
}
