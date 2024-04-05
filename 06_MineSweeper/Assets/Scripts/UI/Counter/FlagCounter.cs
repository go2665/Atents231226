using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCounter : MonoBehaviour
{
    ImageNumber imageNumber;

    private void Awake()
    {
        imageNumber = GetComponent<ImageNumber>();
    }

    private void Start()
    {
        GameManager.Instance.onFlagCountChange += Refresh;
        Refresh(GameManager.Instance.FlagCount);
    }

    void Refresh(int count)
    {
        imageNumber.Number = count;
    }
}
