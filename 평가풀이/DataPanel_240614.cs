using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataPanel_240614 : MonoBehaviour
{
    enum SortType
    {
        ByName = 0,
        ByScore,
        ByRatio,
    }

    List<Data> dataList;

    MyTest inputActions;

    TextMeshProUGUI[] names;
    TextMeshProUGUI[] scores;
    TextMeshProUGUI[] ratios;

    private void Awake()
    {
        inputActions = new MyTest();
        Initialize();
    }

    private void OnEnable()
    {
        inputActions.Test.Enable();
        inputActions.Test.Test1.performed += OnTest1;
        inputActions.Test.Test2.performed += OnTest2;
        inputActions.Test.Test3.performed += OnTest3;
    }

    private void OnDisable()
    {
        inputActions.Test.Test3.performed -= OnTest3;
        inputActions.Test.Test2.performed -= OnTest2;
        inputActions.Test.Test1.performed -= OnTest1;
        inputActions.Test.Disable();
    }

    void Initialize()
    {
        Data d1 = new("AAA", 30, 0.5f);
        Data d2 = new("BB", 10, 0.2f);
        Data d3 = new("CCCC", 40, 0.1f);
        Data d4 = new("DDD", 20, 0.4f);
        Data d5 = new("EE", 50, 0.3f);

        Transform slots = transform.GetChild(1);
        int childCount = slots.childCount;

        names = new TextMeshProUGUI[childCount];
        scores = new TextMeshProUGUI[childCount];
        ratios = new TextMeshProUGUI[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = slots.GetChild(i);
            names[i] = child.GetChild(0).GetComponent<TextMeshProUGUI>();
            scores[i] = child.GetChild(1).GetComponent<TextMeshProUGUI>();
            ratios[i] = child.GetChild(2).GetComponent<TextMeshProUGUI>();
        }

        dataList = new List<Data>(childCount);
        dataList.Add(d1);
        dataList.Add(d2);
        dataList.Add(d3);
        dataList.Add(d4);
        dataList.Add(d5);
    }

    void Sort(SortType type)
    { 
        switch (type)
        {
            case SortType.ByName:
                dataList.Sort((x, y) => x.name.CompareTo(y.name));
                break;
            case SortType.ByScore:
                dataList.Sort((x, y) => x.score.CompareTo(y.score));
                break;
            case SortType.ByRatio:
                dataList.Sort((x, y) => y.ratio.CompareTo(x.ratio));
                break;
        }
    }
    
    void Refresh()
    {
        int i = 0;
        foreach (Data data in dataList)
        {
            names[i].text = data.name;
            scores[i].text = data.score.ToString();
            ratios[i].text = $"{data.ratio:f2}";
            i++;
        }
    }

    private void OnTest1(InputAction.CallbackContext context)
    {
        Sort(SortType.ByName);
        Refresh();
    }

    private void OnTest2(InputAction.CallbackContext context)
    {
        Sort(SortType.ByScore);
        Refresh();
    }

    private void OnTest3(InputAction.CallbackContext context)
    {
        Sort(SortType.ByRatio);
        Refresh();
    }
}