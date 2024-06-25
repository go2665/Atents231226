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

        // 게임 상태 컨트롤러
        private GameStateController _gameStateController = null;

        // 우주선이 배치될 위치
        private SpawnPoint[] _spawnPoints = null;

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;
            // Collect all spawn points in the scene.
            _spawnPoints = FindObjectsOfType<SpawnPoint>();
        }

        // The spawner is started when the GameStateController switches to GameState.Running.
        public void StartSpaceshipSpawner(GameStateController gameStateController)
        {
            _gameIsReady = true;
            _gameStateController = gameStateController;
            foreach (var player in Runner.ActivePlayers)
            {
                SpawnSpaceship(player);
            }
        }

        // Spawns a new spaceship if a client joined after the game already started
        public void PlayerJoined(PlayerRef player)
        {
            if (_gameIsReady == false) return;
            SpawnSpaceship(player);
        }

        // Spawns a spaceship for a player.
        // The spawn point is chosen in the _spawnPoints array using the implicit playerRef to int conversion 
        private void SpawnSpaceship(PlayerRef player)
        {
            // Modulo is used in case there are more players than spawn points.
            int index = player.PlayerId % _spawnPoints.Length;
            var spawnPosition = _spawnPoints[index].transform.position;

            var playerObject = Runner.Spawn(_spaceshipNetworkPrefab, spawnPosition, Quaternion.identity, player);
            // Set Player Object to facilitate access across systems.
            Runner.SetPlayerObject(player, playerObject);

            // Add the new spaceship to the players to be tracked for the game end check.
            _gameStateController.TrackNewPlayer(playerObject.GetComponent<PlayerDataNetworked>().Id);
        }

        // Despawns the spaceship associated with a player when their client leaves the game session.
        public void PlayerLeft(PlayerRef player)
        {
            DespawnSpaceship(player);
        }

        private void DespawnSpaceship(PlayerRef player)
        {
            if (Runner.TryGetPlayerObject(player, out var spaceshipNetworkObject))
            {
                Runner.Despawn(spaceshipNetworkObject);
            }

            // Reset Player Object
            Runner.SetPlayerObject(player, null);
        }
    }
}