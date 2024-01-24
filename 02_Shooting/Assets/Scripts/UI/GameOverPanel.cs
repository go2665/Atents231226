using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    Animator animator;    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Button restart = GetComponentInChildren<Button>();
        restart.onClick.AddListener(() => SceneManager.LoadScene(0));
        //SceneManager.GetActiveScene().buildIndex;   // 현재 열려있는 씬의 인덱스
    }

    private void Start()
    {
        GameManager.Instance.Player.onDie += (_) => animator.SetTrigger("GameOver");
    }
}
