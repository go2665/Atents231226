using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCounter : CounterBase
{
    Timer timer;

    protected override void Awake()
    {
        base.Awake();
        timer = GetComponent<Timer>();
        timer.onTimeChange += Refresh;
    }
}
