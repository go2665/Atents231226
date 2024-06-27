using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using FusionUtilsEvents;

// 세션에 접속한 플레이어의 정보(눈에 보이지 않는 데이터)
public class PlayerData: NetworkBehaviour
{
    // 플레이어 이름(로비에서 설정한 이름)
    [Networked]
    public NetworkString<_16> Nick { get; set; }

    // 플레이어의 인스턴스(실제로 보이는 플레이어 오브젝트)
    [Networked]
    public NetworkObject Instance { get; set; }

    // 델리게이트의 집합(무엇이 등록되는지는 알 수 없음)?
    public FusionEvent OnPlayerDataSpawnedEvent;

    private ChangeDetector _changeDetector; // 네트워크 오브젝트의 값 변경 감지용

    // 이름 설정하는 Rpc(플레이어가 호스트에게 요청)
    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SetNick(string nick)
    {
        Nick = nick;
    }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState, false);  // 네트워크 변수 변경 감지 시작
        if (Object.HasInputAuthority)
        {
            string nickName = PlayerPrefs.GetString("Nick", string.Empty);      // 오너면 PlayerPrefs에서 값 가져오기
            RPC_SetNick(string.IsNullOrEmpty(nickName) ? $"Player {Object.InputAuthority.AsIndex}" : nickName); // 없으면 기본이름, 있으면 설정한 이름
        }

        DontDestroyOnLoad(this);    // 씬 넘어가도 삭제되지 않게 만들기
        Runner.SetPlayerObject(Object.InputAuthority, Object);          // 러너에 플레이어 오브젝트 설정
        OnPlayerDataSpawnedEvent?.Raise(Object.InputAuthority, Runner); // OnPlayerDataSpawnedEvent에 추가되어 있는 델리게이트들 실행

        if (Object.HasStateAuthority)
        {
            // 호스트라면 플레이어 레퍼런스와 이 게임 오브젝트를 연결해 놓기
            GameManager.Instance.SetPlayerDataObject(Object.InputAuthority, this);
        }
    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(Nick):
                    OnPlayerDataSpawnedEvent?.Raise(Object.InputAuthority, Runner); // 이름이 바뀌면 OnPlayerDataSpawnedEvent 실행
                    break;
            }
        }
    }
}
