using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    // 이 탭이 RankPanel에 의해 선택되면 서브 패널을 연다. 선택이 해제되면 서브 패널을 닫는다.
    bool isSelected = false;
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            // 선택되면 버튼의 색이 정상적으로 보이고 선택되지 않으면 반투명하게 보인다.
            // 선택되면 서브 패널을 열고 선택되지 않으면 서브패널을 닫는다.
        }
    }

    void SubPanelOpen()
    {

    }

    void SubPanelClose()
    {

    }
}
