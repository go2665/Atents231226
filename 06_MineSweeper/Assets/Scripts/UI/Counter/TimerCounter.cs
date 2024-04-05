using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCounter : MonoBehaviour
{
    ImageNumber imageNumber;
    Timer timer;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
        timer = GetComponent<Timer>();
        timer.onTimeChange += Refresh;
    }

    void Refresh(int count)
    {
        imageNumber.Number = count;
    }
}
