using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Asteroids.HostSimple
{
    // 플레이어의 정보를 보관하고 다른 모든 클라이언트들에게 전파하는 것을 보장하는 클래스
    public class PlayerDataNetworked : NetworkBehaviour
    {
        // 전역 스테틱 세팅
        private const int STARTING_LIVES = 3;   // 무조건 생명은 3개로 시작

        // 로컬 런타임 참조
        private PlayerOverviewPanel _overviewPanel = null;  // 플레이어 정보 표시하는 UI

        private ChangeDetector _changeDetector; // Networked 데이터 변경 감지용

        // UI에서 사용되는 게임 세션 상세 정보
        // [Networked] 파라메터가 변경되면 항상 OnChanged함수로 넘어간다.
        [HideInInspector]
        [Networked]
        public NetworkString<_16> NickName { get; private set; }    // 플레이어 이름(32바이트 = 16word)

        [HideInInspector]
        [Networked]
        public int Lives { get; private set; }  // 플레이어 생명

        [HideInInspector]
        [Networked]
        public int Score { get; private set; }  // 플레이어 점수

        // 우주선이 스폰되면 실행될 함수
        public override void Spawned()
        {
            // --- Client
            if (Object.HasInputAuthority)   // 이 네트워크 오브젝트를 소유하는(Owner) 사람일 때만
            {
                var nickName = FindObjectOfType<PlayerData>().GetNickName();    // PlayerData에서 플레이어의 이름 가져오기 
                RpcSetNickName(nickName);   // Rpc를 통해 이름 설정하기
            }

            // --- Host
            if (Object.HasStateAuthority)
            {
                Lives = STARTING_LIVES; // 생명과 점수 초기화
                Score = 0;
            }

            // --- Host & Client
            _overviewPanel = FindObjectOfType<PlayerOverviewPanel>();   // PlayerOverviewPanel 찾아서 저장해 두기 
            _overviewPanel.AddEntry(Object.InputAuthority, this);       // 지금 접속한 플레이를 엔트리에 추가
            
            _overviewPanel.UpdateNickName(Object.InputAuthority, NickName.ToString());  // 패널 전체 갱신
            _overviewPanel.UpdateLives(Object.InputAuthority, Lives);
            _overviewPanel.UpdateScore(Object.InputAuthority, Score);
            
            _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState); // Networked 변수들을 감지할 수 있도록 설정
        }
        
        // Update 타이밍에 실행되는 함수
        public override void Render()
        {
            // 변화가 있는 Networked변수들 처리
            foreach (var change in _changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
            {
                switch (change)
                {
                    // 전부 UI만 갱신
                    case nameof(NickName):
                        _overviewPanel.UpdateNickName(Object.InputAuthority, NickName.ToString());
                        break;
                    case nameof(Score):
                        _overviewPanel.UpdateScore(Object.InputAuthority, Score);
                        break;
                    case nameof(Lives):
                        _overviewPanel.UpdateLives(Object.InputAuthority, Lives);
                        break;
                }
            }
        }

        // 이 우주선이 디스폰되면 실행되는 함수
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            _overviewPanel.RemoveEntry(Object.InputAuthority);  // _overviewPanel에서 엔트리 제거
        }

        // 점수를 points만큼 추가
        public void AddToScore(int points)
        {
            Score += points;
        }

        // 생명 1감소 시키는 함수
        public void SubtractLife()
        {
            Lives--;
        }

        // 플레이어의 정보를 오너가 호스트에게 보내는 PRC함수
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RpcSetNickName(string nickName)
        {
            if (string.IsNullOrEmpty(nickName)) return; // 빈 이름은 스킵
            NickName = nickName;    // Networked 변수에 값을 설정하기(RPC를 사용한 이유는 명확하지 않음...)
        }
    }
}