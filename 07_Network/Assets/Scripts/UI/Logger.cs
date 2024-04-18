using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    /// <summary>
    /// 경고 색(노란색)
    /// </summary>
    public Color warningColor;

    /// <summary>
    /// 에러 색(빨간색)
    /// </summary>
    public Color errorColor;

    /// <summary>
    /// 로그창에 출력될 최대 줄 수(안보이는 것 포함)
    /// </summary>
    const int MaxLineCount = 20;

    /// <summary>
    /// 문자열 합치기 위한 스트링 빌더
    /// </summary>
    StringBuilder sb;

    /// <summary>
    /// 로그창에 출력될 모든 문자열 큐
    /// </summary>
    Queue<string> logLines = new Queue<string>(MaxLineCount + 1);

    // 컴포넌트들 --------------------------------------------------------------------
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
            inputField.text = string.Empty;     // 입력 완료되면 비우기
            inputField.ActivateInputField();    // 포커스 다시 활성화
            //inputField.Select();    // 활성화 되어 있을 떄는 비활성화, 비활성화 되어있을 때는 활성화
        });

        child = transform.GetChild(0);
        child = child.GetChild(0);
        child = child.GetChild(0);
        log = child.GetComponent<TextMeshProUGUI>();

        sb = new StringBuilder(MaxLineCount + 1);
    }

    private void Start()
    {
        log.text = string.Empty;
    }

    /// <summary>
    /// 로거에 문자열을 추가하는 함수
    /// </summary>
    /// <param name="message"></param>
    public void Log(string message)
    {
        // 강조할 부분들 강조하기      
        message = HighlightSubString(message, '[', ']', errorColor);    // [] 사이에 있는 글자는 빨간색(errorColor)으로 출력하기
        message = HighlightSubString(message, '{', '}', warningColor);  // {} 사이에 있는 글자는 노란색(warningColor)으로 출력하기

        // 입력 내용을 큐에 넣기(한줄 추가)
        logLines.Enqueue(message);
        if (logLines.Count > MaxLineCount)
        {
            logLines.Dequeue(); // 최대 줄 수를 넘어서면 하나 제거
        }

        // 스트링 빌더로 큐안에 있는 문자열 조합하기
        sb.Clear();
        foreach (string line in logLines)
        {
            sb.AppendLine(line);
        }
        log.text = sb.ToString();
    }

    /// <summary>
    /// 인풋 필드에 포커스를 주는 함수
    /// </summary>
    public void InputFieldFocusOn()
    {
        inputField.ActivateInputField();
    }

    /// <summary>
    /// 지정된 괄호 사이에 있는 글자를 강조하는 함수
    /// </summary>
    /// <param name="source">원문</param>
    /// <param name="open">여는 괄호</param>
    /// <param name="close">닫는 괄호</param>
    /// <param name="color">강조할 부분의 색</param>
    /// <returns>강조가 완료된 문자열</returns>
    string HighlightSubString(string source, char open, char close, Color color)
    {
        string result = source;
        if(IsPair(source, open, close)) // source문자열 안에 있는 괄호가 쌍으로 잘 맞는지 확인(완전히 맞을 때만 강조 처리)
        {
            string[] split = source.Split(open, close);     // 괄호를 기준으로 문자열을 나누기

            string colorText = ColorUtility.ToHtmlStringRGB(color); // 색에 맞게 16진수 문자열 만들기
            
            result = string.Empty;              // IsPair가 실패일 때는 result에 source가 있는게 맞기 때문에 여기서 초기화
            for(int i=0;i<split.Length; i++)    // 나누어 진 것들을 하나씩 처리
            {
                result += split[i];             // 나누어진 문자열을 result에 추가
                if(i != split.Length - 1)       // 마지막 문자열은 제외하고 
                {
                    if( i % 2 == 0 )            
                    {
                        // i가 짝수다 == 이 이후에는 괄호가 열렸을 것이다.
                        result += $"<#{colorText}>{open}";  // 괄호가 열렸을 때부터 색상 변경
                    }
                    else
                    {
                        // i가 홀수다 == 이 이후에는 괄호가 닫혔을 것이다.
                        result += $"{close}</color>";       // 색상 변경 정지
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// source에 지정된 괄호과 정확한 조건으로 들어있는지 체크하는 함수
    /// </summary>
    /// <param name="source">원문</param>
    /// <param name="open">여는 괄호</param>
    /// <param name="close">닫는 괄호</param>
    /// <returns>조건을 모두 만족하면 true, 아니면 false</returns>
    bool IsPair(string source, char open, char close)
    {
        // 정확한 조건 : 열리면 닫혀야 한다. 연속해서 열거나 닫는 것은 금지
        bool result = true;

        int count = 0;  // 괄호 개수
        for(int i=0;i<source.Length;i++)    // source의 모든 글자를 확인
        {
            if (source[i] == open || source[i] == close)    // 여는 괄호이거나 닫는 괄호일 때
            {
                count++;            // 괄호 개수 증가

                if(count % 2 == 1)
                {
                    // count가 홀수면 열려야 한다.
                    if (source[i] != open)
                    {
                        result = false; // 열려야 하는 타이밍인데 열리지 않으면 실패
                        break;
                    }
                }
                else
                {
                    // count가 짝수면 닫혀야 한다.
                    if (source[i] != close)
                    {
                        result = false; // 닫혀야 하는 타이밍인데 닫히지 않으면 실패
                        break;
                    }
                }
            }
        }

        // count는 짝수이어야 열리고 닫힌 쌍이 맞다.
        // count가 0인 경우는 HighlightSubString에서 변경할 필요가 없으니 false
        if ( count % 2 != 0 || count == 0)
        {
            result = false;
        }

        return result;
    }
}
