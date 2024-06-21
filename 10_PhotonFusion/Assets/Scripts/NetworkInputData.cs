using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    public const byte MouseButtonLeft = 1;      // 버튼의 종류
    public const byte MouseButtonRight = 2;

    public NetworkButtons buttons;  // 버튼들의 입력 상황을 담음
    public Vector3 direction;
}