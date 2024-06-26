using Fusion;
using UnityEngine;

namespace Asteroids.HostSimple
{
    // SpaceshipSpawner도 호스트에서만 실행되고 파라메터도 네트워크로 공유될 필요가 없다.
    public class SpaceshipSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        // 플레이어의 우주선용 프리팹(네트워크 오브젝트이어야 한다.)
        [SerializeField] private NetworkPrefabRef _spaceshipNetworkPrefab = NetworkPrefabRef.Empty;

        // 게임이 준비되었는지 표시용(SpaceshipSpawner가 시작되면 true로 설정된다)
        private bool _gameIsReady = false;

        // 게임 상태 컨트롤러(씬전환, 종료처리 등등)
        private GameStateController _gameStateController = null;

        // 우주선이 배치될 위치
        private SpawnPoint[] _spawnPoints = null;

        // 이 네트워크 오브젝트가 네트워크에 생성되면서 실행되는 함수
        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;      // 호스트인지 확인
            
            _spawnPoints = FindObjectsOfType<SpawnPoint>();     // 호스트만 스폰 포인트를 가짐. 씬에서 스폰포인트 찾아 놓기
        }

        // 게임 상태가 러닝 상태가 되면 스포너를 시작 시킨다.
        public void StartSpaceshipSpawner(GameStateController gameStateController)
        {
            _gameIsReady = true;    // 스포너가 준비되었다고 표시
            _gameStateController = gameStateController; // 게임 상태 컨트롤러 저장
            foreach (var player in Runner.ActivePlayers)
            {
                SpawnSpaceship(player); // 모든 활성화된 플레이어에 대해 배를 생성
            }
        }

        // 클라이언트가 게임 시작 이후에 접속하면 새 배 생성하는 함수
        public void PlayerJoined(PlayerRef player)
        {
            if (_gameIsReady == false) return;  // 스포너가 레디 상태가 아니면 스킵
            SpawnSpaceship(player);             // 해당 플레이어에 대한 배 생성
        }

        // 플레이어를 위한 우주선을 스폰하는 함수(스폰위치는 playerRef로 결정)
        private void SpawnSpaceship(PlayerRef player)
        {
            // Modulo연산을 이용해서 플레이어의 스폰위치 결정
            int index = player.PlayerId % _spawnPoints.Length;
            var spawnPosition = _spawnPoints[index].transform.position;

            // 우주선 스폰
            var playerObject = Runner.Spawn(_spaceshipNetworkPrefab, spawnPosition, Quaternion.identity, player);
            Runner.SetPlayerObject(player, playerObject);   // PlayerRef와 생성한 네트워크 오브젝트를 연결하기?

            // 새 우주선을 _gameStateController가 추적할 수 있도록 추가
            _gameStateController.TrackNewPlayer(playerObject.GetComponent<PlayerDataNetworked>().Id);
        }

        // 클라이언트가 게임 세션을 떠나면 실행되는 함수(호스트에서만 실행됨)
        public void PlayerLeft(PlayerRef player)
        {
            // PlayerLeft는 호스트에서만 발생한다.
            DespawnSpaceship(player);   // 나간 플레이어 디스폰 시키기            
        }

        // 우주선을 디스폰시키는 함수
        private void DespawnSpaceship(PlayerRef player)
        {
            if (Runner.TryGetPlayerObject(player, out var spaceshipNetworkObject))  // 러너에 잇는지 확인하고
            {
                Runner.Despawn(spaceshipNetworkObject); // 있으면 디스폰
            }

            Runner.SetPlayerObject(player, null);   // 플레이어 오브젝트를 null로 리셋
        }
    }
}