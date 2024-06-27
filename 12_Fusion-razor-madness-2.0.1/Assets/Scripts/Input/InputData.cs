using Fusion;
using UnityEngine;

[System.Flags]
public enum InputButton
{
    LEFT = 1 << 0,          // 0000 0001
    RIGHT = 1 << 1,         // 0000 0010
    RESPAWN = 1 << 3,       // 0000 1000
    JUMP = 1 << 4,          // 0001 0000
}

public struct InputData : INetworkInput
{
    public NetworkButtons Buttons;

    // 특정 버튼이 지금 눌려져있는지 확인하는 함수
    public bool GetButton(InputButton button)
    {
        return Buttons.IsSet(button);
    }

    // 모든 버튼에 대해서 이전에 안눌려져 있었고 지금 눌려져 있는 비트만 1로 세팅
    public NetworkButtons GetButtonPressed(NetworkButtons prev)
    {
        return Buttons.GetPressed(prev);
    }

    // 왼쪽 또는 오른쪽이 눌려져 있으면 true
    public bool AxisPressed()
    {
        return GetButton(InputButton.LEFT) || GetButton(InputButton.RIGHT);
    }
}
