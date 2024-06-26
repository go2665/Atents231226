using Fusion;
using UnityEngine;

namespace Asteroids.HostSimple
{
    // 총알의 행동 정의
    public class BulletBehaviour : NetworkBehaviour
    {
        // 최대 수명(3초)
        [SerializeField] private float _maxLifetime = 3.0f;

        // 이동 속도
        [SerializeField] private float _speed = 200.0f;

        // 운석 레이어
        [SerializeField] private LayerMask _asteroidLayer;

        // 수명 타이머
        [Networked] private TickTimer _currentLifetime { get; set; }

        public override void Spawned()
        {
            if (Object.HasStateAuthority == false) return;
            
            _currentLifetime = TickTimer.CreateFromSeconds(Runner, _maxLifetime);   // 총알의 수명 타이머 설정
        }

        public override void FixedUpdateNetwork()
        {
            // 운석과 부딪치지 않았으면 계속 앞으로 간다.
            if (HasHitAsteroid() == false)
            {
                transform.Translate(_speed * Runner.DeltaTime * transform.forward, Space.World);
            }
            else
            {
                Runner.Despawn(Object); // 부딪쳤으면 디스폰하고 리턴
                return;
            }

            CheckLifetime();    // 수명 체크하기
        }

        // 수명이 만료되면 디스폰
        private void CheckLifetime()
        {
            if (_currentLifetime.Expired(Runner) == false) return;  // 수명이 남아있으면 그냥 스킵

            Runner.Despawn(Object); // 수명이 다 됬으면 디스폰
        }

        // 다음 틱에 운석과 충돌할 것인지 체크하는 함수
        private bool HasHitAsteroid()
        {
            // LagCompensation를 통해 네트워크 통신으로 인해 발생하는 랙을 보정한다.
            var hitAsteroid = Runner.LagCompensation.Raycast(
                transform.position,         // 레이의 원점
                transform.forward,          // 레이의 방향
                _speed * Runner.DeltaTime,  // 레이의 길이(이 프레임에 이동할 거리)
                Object.InputAuthority,      // 이 총알을 발사한 플레이어
                out var hit,                // hit 관련 정보
                _asteroidLayer);            // 운석의 레이어

            if (hitAsteroid == false) return false; // 충돌 안했으면 리턴 false

            var asteroidBehaviour = hit.GameObject.GetComponent<AsteroidBehaviour>();

            if (asteroidBehaviour.IsAlive == false) // 이미 터진 운석이면 리턴 false
                return false;

            asteroidBehaviour.HitAsteroid(Object.InputAuthority);   // 운석 명중 처리

            return true;
        }
    }
}
