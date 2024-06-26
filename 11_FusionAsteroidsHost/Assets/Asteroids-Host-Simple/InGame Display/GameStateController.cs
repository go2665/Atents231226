using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using TMPro;
using UnityEngine;

namespace Asteroids.HostSimple
{
    public class GameStateController : NetworkBehaviour
    {
        // 게임 상태 종류
        enum GameState
        {
            Starting,   // 시작 중(시작 딜레이 카운팅 되는 시점)
            Running,    // 플레이 중
            Ending      // 끝났을 때(종료 딜레이 카운팅 되는 시점)
        }

        // 게임이 시작되었을 때의 딜레이
        [SerializeField] private float _startDelay = 4.0f;
        // 게임이 종료되었을 때의 딜레이
        [SerializeField] private float _endDelay = 4.0f;
        // 게임의 세션 길이(딱히 사용되고 있지 않음)
        [SerializeField] private float _gameSessionLength = 180.0f;

        // 시작/종료 딜레이 표시용(인스펙터 창에서 따로 추가)
        [SerializeField] private TextMeshProUGUI _startEndDisplay = null;
        // 세션 딜레이 표시용(인스펙터 창에서 따로 추가)
        [SerializeField] private TextMeshProUGUI _ingameTimerDisplay = null;

        [Networked] private TickTimer _timer { get; set; }          // 타이머(시작/종료/세션 모두 사용)
        [Networked] private GameState _gameState { get; set; }      // 게임 상태

        [Networked] private NetworkBehaviourId _winner { get; set; }    // 승자의 네트워크 아이디

        private List<NetworkBehaviourId> _playerDataNetworkedIds = new List<NetworkBehaviourId>();  // 모든 플레이어의 네트워크 아이디

        // 스폰 이후에 실행되는 함수
        public override void Spawned()
        {
            // --- This section is for all information which has to be locally initialized based on the networked game state
            // --- when a CLIENT joins a game
            // 이 부분은 게임 상태에 따라 로컬로 초기화해야하는 모든 정보에 대한 것?(클라이언트가 게임에 접속했을 때)
                        
            _startEndDisplay.gameObject.SetActive(true);        // 가운데 텍스트 활성화
            _ingameTimerDisplay.gameObject.SetActive(false);    // 아래쪽 텍스트 비활성화

            // 이미 게임이 시작된 상황(다른 사람이 플레이 중인 방에 접속한 상황)이면
            if (_gameState != GameState.Starting)
            {
                foreach (var player in Runner.ActivePlayers)    // 모든 활성화된 플레이어를 순회하면서
                {
                    if (Runner.TryGetPlayerObject(player, out var playerObject) == false) continue;
                    TrackNewPlayer(playerObject.GetComponent<PlayerDataNetworked>().Id);    // 리스트에 플레이어를 추가한다?
                }
            }

            // GameStateController는 클라이언트에서도 FixedUpdateNetwork를 실행하게 한다?
            Runner.SetIsSimulated(Object, true);

            // --- 여기서 부터는 호스트에 의해 초기화되는 모든 [networked] 변수의 초기화 작업
            if (Object.HasStateAuthority == false) return;

            _gameState = GameState.Starting;                            // 상태 변경
            _timer = TickTimer.CreateFromSeconds(Runner, _startDelay);  // 시작 딜레이 설정
        }

        public override void FixedUpdateNetwork()
        {
            // 게임 상태에 따른 게임 화면 업데이트
            switch (_gameState)
            {
                case GameState.Starting:
                    UpdateStartingDisplay();
                    break;
                case GameState.Running:
                    UpdateRunningDisplay();
                    if (_timer.ExpiredOrNotRunning(Runner))     // _gameSessionLength 시간이 만료되면 게임 종료
                    {
                        // _playerDataNetworkedIds를 순회하면서 점수가 가장 높은 사람을 승리자로 선정
                        PlayerDataNetworked winner;
                        Runner.TryFindBehaviour(_playerDataNetworkedIds[0], out winner);    // 우선 첫번째 사람을 무조건 승리자로 설정하기

                        for (int i = 1; i < _playerDataNetworkedIds.Count; i++)     // 남은 사람들 순회하기
                        {
                            if (Runner.TryFindBehaviour(_playerDataNetworkedIds[i],
                                    out PlayerDataNetworked playerDataNetworkedComponent))
                            {
                                if( winner.Score <= playerDataNetworkedComponent.Score)     // 순회중에 승리자보다 점수가 높은 사람이 있으면 승리자 변경
                                {
                                    winner = playerDataNetworkedComponent;
                                }
                            }
                        }
                        _winner = winner.Id;

                        GameHasEnded(); 
                    }
                    break;
                case GameState.Ending:
                    UpdateEndingDisplay();  // 버그 : 상태는 end로 바뀌는데 세션 시간이 만료되었을 때 Winner가 설정이 안되어 있음
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateStartingDisplay()
        {
            // --- Host & Client

            // 게임 시작까지 남아있는 시간 출력(소수점 없이)
            _startEndDisplay.text = $"Game Starts In {Mathf.RoundToInt(_timer.RemainingTime(Runner) ?? 0)}";

            // --- Host
            // 호스트가 아니면 이후는 진행하지 말것
            if (Object.HasStateAuthority == false) return;
            // 시작 딜레이 타이머가 만료되지 않았고 동작 중일 때 이후는 진행하지 말 것
            if (_timer.ExpiredOrNotRunning(Runner) == false) return;
            
            // 여기부터는 호스트이면서 타이머가 만료되거나 실행되고 있지 않을 때 처리
            
            // 우주선과 운석 스포너를 작동시킨다(게임 시작 딜레이가 만료되면 한번만 실행된다)
            FindObjectOfType<SpaceshipSpawner>().StartSpaceshipSpawner(this);
            FindObjectOfType<AsteroidSpawner>().StartAsteroidSpawner();

            _gameState = GameState.Running;                                     // 게임 상태를 Running으로 변경
            _timer = TickTimer.CreateFromSeconds(Runner, _gameSessionLength);   // 타이머를 세션 길이로 재시작
        }

        private void UpdateRunningDisplay()
        {
            // --- Host & Client
            _startEndDisplay.gameObject.SetActive(false);       // 가운데 텍스트 비활성화
            _ingameTimerDisplay.gameObject.SetActive(true);     // 아래쪽 텍스트 활성화
            _ingameTimerDisplay.text =
                $"{Mathf.RoundToInt(_timer.RemainingTime(Runner) ?? 0).ToString("000")} seconds left";  // 남은 세션 시간을 초단위로 출력
        }

        private void UpdateEndingDisplay()
        {
            // --- Host & Client
            // 게임 결과와 셧다운까지 남은시간 출력

            if (Runner.TryFindBehaviour(_winner, out PlayerDataNetworked playerData) == false) return;  // 승리자가 없으면 종료

            _startEndDisplay.gameObject.SetActive(true);            // 가운데 텍스트 활성화
            _ingameTimerDisplay.gameObject.SetActive(false);        // 아래쪽 텍스트 비활성화
            _startEndDisplay.text =                                 // 승리자와 승리자의 점수, 디스커넥트까지 남은 시간 출력
                $"{playerData.NickName} won with {playerData.Score} points. Disconnecting in {Mathf.RoundToInt(_timer.RemainingTime(Runner) ?? 0)}";
            _startEndDisplay.color = SpaceshipVisualController.GetColor(playerData.Object.InputAuthority.PlayerId); // 승리자의 색상으로 글자색 변경

            // --- Host
            // 세임 세션 셧다운 하기
            if (_timer.ExpiredOrNotRunning(Runner) == false) return;    // 게임 종료 타이머가 만료될때까지 스킵

            Runner.Shutdown();  // 게임 종료 타이머가 만료되면 셧다운 진행
        }

        // 게임이 종료될 상황인지 체크하는 함수(배가 운석에 맞았을 때 실행되는 함수)
        public void CheckIfGameHasEnded()
        {
            if (Object.HasStateAuthority == false) return;  // 호스트가 아니면 리턴

            // 호스트만 처리
            int playersAlive = 0;

            for (int i = 0; i < _playerDataNetworkedIds.Count; i++) // _playerDataNetworkedIds를 순회하면서 러너에 없는 것은 제거
            {
                if (Runner.TryFindBehaviour(_playerDataNetworkedIds[i],
                        out PlayerDataNetworked playerDataNetworkedComponent) == false)
                {
                    _playerDataNetworkedIds.RemoveAt(i);
                    i--;
                    continue;
                }

                if (playerDataNetworkedComponent.Lives > 0) playersAlive++; // 순회하면서 수명이 1이상인 플레이어 수 카운팅
            }

            // 플레이어가 2명 이상 남아있거나, 혼자서 플레이 하는 경우인데 수명이 남아있으면 게임 계속 진행
            if (playersAlive > 1 || (Runner.ActivePlayers.Count() == 1 && playersAlive == 1)) return;


            foreach (var playerDataNetworkedId in _playerDataNetworkedIds)
            {
                if (Runner.TryFindBehaviour(playerDataNetworkedId,
                        out PlayerDataNetworked playerDataNetworkedComponent) ==
                    false) continue;    // 목록에 없으면 스킵

                if (playerDataNetworkedComponent.Lives > 0 == false) continue;  // 수명 다 된 사람은 스킵

                _winner = playerDataNetworkedId;    // 승리자 결정
            }

            // _winner가 값이 없으면 혼자서 호스트 모드로 플레이 하고 있다고 가정하고 승리자로 설정
            if (_winner == default) 
            {
                _winner = _playerDataNetworkedIds[0];
            }

            GameHasEnded(); // 실제 게임 종료 처리
        }

        // 게임 종료될 때 실행될 함수
        private void GameHasEnded()
        {
            _timer = TickTimer.CreateFromSeconds(Runner, _endDelay);    // 종료 딜레이 설정
            _gameState = GameState.Ending;  // 게임 상태를 Ending으로 변경
        }

        // 새 플레이어를 리스트에 추가(종료 체크용)
        public void TrackNewPlayer(NetworkBehaviourId playerDataNetworkedId)
        {
            _playerDataNetworkedIds.Add(playerDataNetworkedId);
        }
    }
}
