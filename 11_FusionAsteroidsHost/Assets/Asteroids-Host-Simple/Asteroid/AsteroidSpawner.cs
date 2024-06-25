using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace Asteroids.HostSimple
{
    // AsteroidSpawner는 호스트에서만 실행된다.
    // 그래서 모든 파라메터는 로컬에만 있고 네트워크로 공유되지 않는다.(호스트에서만 실행되니까)
    public class AsteroidSpawner : NetworkBehaviour
    {
        // 크고 작은 운석의 네트워크 오브젝트 프리팹(NetworkObject컴포넌트를 가지고 있어야만 NetworkPrefabRef에 할당 가능)
        [SerializeField] private NetworkPrefabRef _smallAsteroid = NetworkPrefabRef.Empty;
        [SerializeField] private NetworkPrefabRef _bigAsteroid = NetworkPrefabRef.Empty;

        // 큰 운석 스폰 주기(5~10초로 초기 설정)
        [SerializeField] private float _minSpawnDelay = 5.0f;
        [SerializeField] private float _maxSpawnDelay = 10.0f;

        // 큰 운석이 파괴 되었을 때 작은 운석의 생성 개수(3~6개)
        [SerializeField] private int _minAsteroidSplinters = 3;
        [SerializeField] private int _maxAsteroidSplinters = 6;

        // 스폰 시간 간격 처리용 TickTimer
        private TickTimer _spawnDelay;

        // 스폰 영역(카메라 세팅에 기반함)
        private float _screenBoundaryX = 0.0f;
        private float _screenBoundaryY = 0.0f;

        // 생성된 운석의 네트워크 아이디의 리스트
        private List<NetworkId> _asteroids = new List<NetworkId>();

        // 스포너는 GameStateController가 상태를 Running으로 만들 때 시작된다(시작 딜레이가 0이 되었을때)
        public void StartAsteroidSpawner()
        {
            if (Object.HasStateAuthority == false) return;  // 호스트만 뒤쪽 코드 진행 가능

            SetSpawnDelay();    // 첫번째 스폰 딜레이 설정

            // 스폰 바운더리 계산하기(카메라 세팅 기반)
            _screenBoundaryX = Camera.main.orthographicSize * Camera.main.aspect;
            _screenBoundaryY = Camera.main.orthographicSize;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority == false) return;  // 호스트만 뒤쪽 코드 진행 가능

            SpawnAsteroid();                // 큰 운석 스폰 시도

            CheckOutOfBoundsAsteroids();
        }

        // _spawnDelay가 만료되면 큰 운석을 스폰하는 함수
        private void SpawnAsteroid()
        {
            if (_spawnDelay.Expired(Runner) == false) return;   // _spawnDelay가 만료되지 않았으면 return

            // _spawnDelay가 만료되었다.

            Vector2 direction = Random.insideUnitCircle;    // 랜덤한 방향 벡터 구하기
            Vector3 position = Vector3.zero;                // 위치는 (0,0,0)

            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // 왼쪽면과 오른쪽면 위의 한 위치를 구함
                position = new Vector3(Mathf.Sign(direction.x) * _screenBoundaryX, 0, direction.y * _screenBoundaryY);

                // Mathf.Sign(f) : f가 +인지 -인지 구하는 함수. f가 +면 1, f가 -면 -1을 리턴
            }
            else
            {
                // 위쪽면과 아래쪽면의 한 위치를 구함
                position = new Vector3(direction.x * _screenBoundaryX, 0, Mathf.Sign(direction.y) * _screenBoundaryY);
            }

            // 바운더리 선상에 있던 위치를 약간 화면 안쪽으로 이동시키기
            // 만들어졌을 때 바로 화면을 나간것으로 체크해 파괴되는 것을 방지
            position -= position.normalized * 0.1f;

            // x,y,z 전부 랜덤하게 회전 생성
            var rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f),
                Random.Range(0.0f, 360.0f));

            // 큰 운석 스폰
            var asteroid = Runner.Spawn(_bigAsteroid, position, rotation, PlayerRef.None,
                onBeforeSpawned: SpinBigAsteroid);

            // 스폰한 운석의 네트워크 아이디를 리스트에 기록해 두기(화면 밖으로 나간 운석 제거 체크용)
            _asteroids.Add(asteroid.Id);

            // 다시 스폰 딜레이 설정
            SetSpawnDelay();
        }

        /// <summary>
        /// 스폰 딜레이 설정하는 함수
        /// </summary>
        private void SetSpawnDelay()
        {
            var time = Random.Range(_minSpawnDelay, _maxSpawnDelay);    // 최소~최대 딜레이 변수에서 랜덤으로 시간 결정
            _spawnDelay = TickTimer.CreateFromSeconds(Runner, time);    // _spawnDelay 틱타이머를 새로 설정
        }

        // 운석이 게임 바운더리 밖으로 나갔는지 체크하고 나갔으면 디스폰 처리
        private void CheckOutOfBoundsAsteroids()
        {
            for (int i = 0; i < _asteroids.Count; i++)  // 모든 운석 목록을 순회하기
            {
                // 네트워크 상에서 운석이 있는지 확인
                if (Runner.TryFindObject(_asteroids[i], out var asteroid) == false)
                {
                    // 없다
                    _asteroids.RemoveAt(i);     // 리스트에서 제거
                    i--;                        // i를 1감소(위에서 빠진 것에 맞게)
                    continue;   // 다음 루프로 진행
                }

                if (IsWithinScreenBoundary(asteroid.transform.position)) continue;  // 위치가 화면 바운더리 안이면 다음 루프로 진행

                // 네트워크 상에 운석이 있고, 위치가 화면 바운더리 밖이다.
                Runner.Despawn(asteroid);   // 디스폰 처리
                i--;    // i 1감소(아마 러너에서 빠졌을 때 처리하기 위한 것으로 추정)                        
            }
        }

        // 입력 받은 위치가 화면 바운더리 안에 있는지 확인
        private bool IsWithinScreenBoundary(Vector3 asteroidPosition)
        {
            return Mathf.Abs(asteroidPosition.x) < _screenBoundaryX && Mathf.Abs(asteroidPosition.z) < _screenBoundaryY;
        }

        // 큰 운석이 스폰 되기 직전에 초기화하는 함수
        // 큰 운석에 이동 방향과 회전력 추가, 큰 운석 표시하는 함수
        private void SpinBigAsteroid(NetworkRunner runner, NetworkObject asteroidNetworkObject)
        {
            Vector3 force = -asteroidNetworkObject.transform.position.normalized * 1000.0f; // 화면 중심으로 향하는 벡터 구하기
            Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);       // 랜덤한 회전력 구하기

            var rb = asteroidNetworkObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;     // 풀 오브젝트니까 초기화
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(force);             // 화면 중심쪽을 향해 힘 추가
            rb.AddTorque(torque);           // 회전력 추가

            var asteroidBehaviour = asteroidNetworkObject.GetComponent<AsteroidBehaviour>();
            asteroidBehaviour.IsBig = true; // 큰 운석이라고 표시
        }

        // 작은 운석 초기화(이동 방향, 회전 방향, 작은 운석 표시 추가)
        private void SpinSmallAsteroid(NetworkRunner runner, NetworkObject asteroidNetworkObject, Vector3 force,
            Vector3 torque)
        {
            var rb = asteroidNetworkObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero; // 초기화
            rb.AddForce(force);         // 힘 추가
            rb.AddTorque(torque);       // 회전력 추가

            var asteroidBehaviour = asteroidNetworkObject.GetComponent<AsteroidBehaviour>();
            asteroidBehaviour.IsBig = false;    // 작은 우석이라고 표시
        }

        // 큰 운석이 파괴 되었을 때 랜덤 한 수의 작은 운석 스폰
        public void BreakUpBigAsteroid(Vector3 position)
        {
            int splintersToSpawn = Random.Range(_minAsteroidSplinters, _maxAsteroidSplinters);  // 작은 운석의 갯수 결정

            for (int counter = 0; counter < splintersToSpawn; ++counter)
            {
                // 사방으로 퍼져 나가는 힘 방향 구하기
                // Vector3.forward 벡터를 Quaternion.Euler(0, counter * 360.0f / splintersToSpawn, 0)만큼 회전 시키고
                // 크기를 150~450으로 증폭시키기
                Vector3 force = Quaternion.Euler(0, counter * 360.0f / splintersToSpawn, 0) * Vector3.forward *
                                Random.Range(0.5f, 1.5f) * 300.0f;

                // 회전력 랜덤으로 구하기
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);

                // 초기 회전 방향 구하기(다양하게 보여주기 위한 목적)
                Quaternion rotation = Quaternion.Euler(0, Random.value * 180.0f, 0);

                // 작은 운석 스폰
                var asteroid = Runner.Spawn(_smallAsteroid, position + force.normalized * 10.0f, rotation,
                    PlayerRef.None,
                    (networkRunner, asteroidNetworkObject) =>
                        SpinSmallAsteroid(networkRunner, asteroidNetworkObject, force, torque));

                _asteroids.Add(asteroid.Id);    // 리스트에 추가
            }
        }
    }
}
