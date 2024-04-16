using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_RankingData : TestBase
{
    public int action;
    public float time;
    public string rankerName = "Test";

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.RankDataManager.Test_RankSetting();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.RankDataManager.Test_ActionUpdate(action, rankerName);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.RankDataManager.Test_TimeUpdate(time, rankerName);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        GameManager.Instance.RankDataManager.Test_Save();
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        GameManager.Instance.RankDataManager.Test_Load();
    }
}
