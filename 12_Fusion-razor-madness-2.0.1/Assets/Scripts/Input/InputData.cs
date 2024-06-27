using Fusion;
using UnityEngine;

[System.Flags]
public enum InputButton
{
    LEFT = 1 << 0,
    RIGHT = 1 << 1,
    RESPAWN = 1 << 3,
    JUMP = 1 << 4,
}

public struct InputData : INetworkInput
{
    public NetworkButtons Buttons;

    public bool GetButton(InputButton button)
    {
        return Buttons.IsSet(button);
    }

    public NetworkButtons GetButtonPressed(NetworkButtons prev)
    {
        return Buttons.GetPressed(prev);
    }

    public bool AxisPressed()
    {
        return GetButton(InputButton.LEFT) || GetButton(InputButton.RIGHT);
    }
}
