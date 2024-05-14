using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public Color userColor;
    public Color enemyColor;
    public Color shipColor;
    public Color turnColor;

    // 실습
    // 1. public 함수 없음(테스트는 예외)
    // 2. 턴 시작시에 몇번째 턴이 시작되었는지 출력하기(턴 번호는 다른 색으로 출력하기)
    // 3. 공격시에 공격 성공/실패 출력하기(공격 실패 델리게이트 필요)
    //  3.1. 성공 예시) : "[적]의 공격 : [당신]의 [항공모함]에 포탄이 명중했습니다."
    //  3.2. 실패 예시) : "[나]의 공격 : [나]의 포탄이 빗나갔습니다."
    // 4. 함선이 침몰하면 침몰했다고 출력하기
    //  4.1. 예시) "[나]의 공격 : [적]의 [구축함]이 침몰했습니다."
    // 5. 게임이 종료되면 상황 출력하기
    //  5.1. 예시) "[당신]의 승리!"
    //  5.2. 예시) "[당신]의 패배..."

    TextMeshProUGUI log;
    
    const int MaxLineCount = 20;
    const string YOU = "당신";
    const string ENEMY = "적";

    List<string> lines;
    StringBuilder builder;

    private void Awake()
    {
        log = GetComponentInChildren<TextMeshProUGUI>();
        lines = new List<string>(MaxLineCount + 1);
        builder = new StringBuilder(MaxLineCount + 1);
    }

    private void Start()
    {
        GameManager gameManager = GameManager.Instance;
        gameManager.TurnController.onTurnStart += Log_TurnStart;    // 턴 시작 델리게이트 받기

        UserPlayer user = gameManager.UserPlayer;
        EnemyPlayer enemy = gameManager.EnemyPlayer;
        foreach (Ship ship in user.Ships)
        {
            ship.onHit += (target) => Log_AttackSuccess(false, target); // 유저의 함선이 명중되었으니 적의 공격이 성공한 것
            ship.onSink = (target) => { Log_ShipSinking(false, target); } + ship.onSink;    // 로그를 먼저 찍도록 연결
        }
        foreach (Ship ship in enemy.Ships)
        {
            ship.onHit += (target) => Log_AttackSuccess(true, target); // 적의 함선이 명중되었으니 유저의 공격이 성공한 것
            ship.onSink = (target) => { Log_ShipSinking(true, target); } + ship.onSink;
        }
        user.onAttackFail += Log_AttackFail;
        enemy.onAttackFail += Log_AttackFail;

        user.onDefeat += () => Log_Defeat(true);
        enemy.onDefeat += () => Log_Defeat(false);

        Clear();

        Log_TurnStart(1);   // 턴 매니저와의 순서 문제로 따로 실행
    }

    void Log(string text)
    {
        lines.Add(text);
        if(lines.Count > MaxLineCount)
        {
            lines.RemoveAt(0);
        }

        builder.Clear();
        foreach(string line in lines)
        {
            builder.AppendLine(line);
        }

        log.text = builder.ToString();
    }

    void Clear()
    {
        lines.Clear();
        log.text = string.Empty;
    }


    void Log_TurnStart(int number)
    {
        string colorText = ColorUtility.ToHtmlStringRGB(turnColor);
        Log($"<b><#{colorText}>{number}</color></b> 번째 턴이 시작되었습니다.");
    }

    void Log_AttackSuccess(bool isUser, Ship ship)
    {
        // "[적]의 공격 : [당신]의 [항공모함]에 포탄이 명중했습니다."
        string attackerName;
        string attackerColor;
        string hitterName;
        string hitterColor;
        string shipTextColor;

        if(isUser)
        {
            attackerName = YOU;
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            hitterName = ENEMY;
            hitterColor = ColorUtility.ToHtmlStringRGB(enemyColor);
        }
        else
        {
            attackerName = ENEMY;
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            hitterName = YOU;
            hitterColor = ColorUtility.ToHtmlStringRGB(userColor);
        }
        shipTextColor = ColorUtility.ToHtmlStringRGB(shipColor);

        Log($"<b><#{attackerColor}>{attackerName}</color></b>의 공격\t: <b><#{hitterColor}>{hitterName}</color></b>의 <b><#{shipTextColor}>{ship.ShipName}</color></b>에 명중");
    }

    void Log_AttackFail(bool isUser)
    {
        // "[나]의 공격 : [나]의 포탄이 빗나갔습니다."
        string attackerName;
        string attackerColor;

        if (isUser)
        {
            attackerName = YOU;
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
        }
        else
        {
            attackerName = ENEMY;
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
        }
        Log($"<b><#{attackerColor}>{attackerName}</color></b>의 공격\t: <b><#{attackerColor}>{attackerName}</color></b>의 포탄이 빗나갔습니다."); 
    }

    void Log_ShipSinking(bool isUser, Ship ship)
    {
        //"[나]의 공격 : [적]의 [구축함]이 침몰했습니다."
        string attackerName;
        string attackerColor;
        string hitterName;
        string hitterColor;
        string shipTextColor;

        if (isUser)
        {
            attackerName = YOU;
            attackerColor = ColorUtility.ToHtmlStringRGB(userColor);
            hitterName = ENEMY;
            hitterColor = ColorUtility.ToHtmlStringRGB(enemyColor);
        }
        else
        {
            attackerName = ENEMY;
            attackerColor = ColorUtility.ToHtmlStringRGB(enemyColor);
            hitterName = YOU;
            hitterColor = ColorUtility.ToHtmlStringRGB(userColor);
        }
        shipTextColor = ColorUtility.ToHtmlStringRGB(shipColor);

        Log($"<b><#{attackerColor}>{attackerName}</color></b>의 공격\t: <b><#{hitterColor}>{hitterName}</color></b>의 <b><#{shipTextColor}>{ship.ShipName}</color></b>이 침몰했습니다.");
    }

    void Log_Defeat(bool isUser)
    {
        // "[당신]의 승리!"
        // "[당신]의 패배..."

        string temp = $"<b><#{ColorUtility.ToHtmlStringRGB(userColor)}>{YOU}</color></b>의 ";
        if (isUser)
        {
            // 내가 졌다.
            Log($"{temp}패배...");
        }
        else
        {
            // 적이 졌다. => 내가 이겼다.
            Log($"{temp}승리!");
        }
    }
}
