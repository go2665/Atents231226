using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    public Color warningColor;  // 노란색
    public Color errorColor;    // 빨간색

    const int MaxLineCount = 20;

    StringBuilder sb;

    Queue<string> logLines = new Queue<string>(MaxLineCount + 1);

    TextMeshProUGUI log;
    TMP_InputField inputField;

    private void Awake()
    {
        Transform child = transform.GetChild(3);
        inputField = child.GetComponent<TMP_InputField>();
        // onEndEdit;   // 입력이 끝났을 때 실행(엔터치거나 포커스를 잃었을 때 실행)
        // onSubmit;    // 입력이 완료되었을 때 실행(엔터쳤을 때만 실행)
        inputField.onSubmit.AddListener((text) =>
        {
            Log(text);
            inputField.text = string.Empty;
            inputField.ActivateInputField();    // 포커스 다시 활성화
            //inputField.Select();    // 활성화 되어 있을 떄는 비활성화, 비활성화 되어있을 때는 활성화
        });

        child = transform.GetChild(0);
        child = child.GetChild(0);
        child = child.GetChild(0);
        log = child.GetComponent<TextMeshProUGUI>();

        sb = new StringBuilder(MaxLineCount + 1);
    }

    public void Log(string message)
    {
        // "[위험] {경고}"
        // [] 사이에 있는 글자는 빨간색(errorColor)으로 출력하기
        // {} 사이에 있는 글자는 노란색(warningColor)으로 출력하기
                

        logLines.Enqueue(message);
        if (logLines.Count > MaxLineCount)
        {
            logLines.Dequeue();
        }

        sb.Clear();
        foreach (string line in logLines)
        {
            sb.AppendLine(line);
        }

        log.text = sb.ToString();
    }

    public void InputFieldFocusOn()
    {
        inputField.ActivateInputField();
    }
}
