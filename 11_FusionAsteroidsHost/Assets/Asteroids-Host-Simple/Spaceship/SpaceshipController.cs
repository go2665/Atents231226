using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.LagCompensation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Asteroids.HostSimple
{
    // 우주선의 생명주기를 제어하는 클래스
    public class SpaceshipController : NetworkBehaviour
    {
        // 게임 세션용 "비공식" 세팅들
        // 리스폰 딜레이
        [SerializeField] private float _respawnDelay = 4.0f;
        // 운석과의 충돌 반경
        [SerializeField] private float _spaceshipDamageRadius = 2.5f;
        // 운석 레이어
        [SerializeField] private LayerMask _asteroidCollisionLayer;

        // Local Runtime references
        private ChangeDetector _changeDetector;
        private Rigidbody _rigidbody = null;
        private PlayerDataNetworked _playerDataNetworked = null;
        private SpaceshipVisualController _visualController = null;

        // 우주선과 충돌한 대상에 대한 정보들(= 충돌한 운석들에 대한 정보)
        private List<LagCompensatedHit> _lagCompensatedHits = new List<LagCompensatedHit>();

        // 게임 세션 상세 세팅
        // 살아있고 오브젝트도 valid하면 true(리스폰 시간이나 게임 오버 이후에 입력 방지?)
        public bool AcceptInput => _isAlive && Object.IsValid;

        // 우주선 생존 여부
        [Networked] private NetworkBool _isAlive { get; set; }

        // 리스폰 타이머
        [Networked] private TickTimer _respawnTimer { get; set; }

        public override void Spawned()
        {
            // --- Host & Client
            // local runtime references 설정하기
            _rigidbody = GetComponent<Rigidbody>();
            _playerDataNetworked = GetComponent<PlayerDataNetworked>();
            _visualController = GetComponent<SpaceshipVisualController>();
            _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

            _visualController.SetColorFromPlayerID(Object.InputAuthority.PlayerId); // 비주얼 컨트롤러로 플레이어 색상 설정

            // --- Host
            if (Object.HasStateAuthority == false) return;
            _isAlive = true;    // 살아있다고 표시(호스트만 실행)
        }

        public override void Render()
        {
            foreach (var change in _changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
            {
                switch (change)
                {
                    case nameof(_isAlive):  // _isAlive에 변화가 있는지 체크
                        var reader = GetPropertyReader<NetworkBool>(nameof(_isAlive));          // 리더 가져오기
                        var (previous, current) = reader.Read(previousBuffer, currentBuffer);   // 리더에서 값을 읽기
                        ToggleVisuals(previous, current);   // ToggleVisuals에 읽은 값 넘겨주기
                        break;
                }
            }
        }

        // 우주선의 모습을 토글 시키기
        private void ToggleVisuals(bool wasAlive, bool isAlive)
        {
            // Check if the spaceship was just brought to life
            if (wasAlive == false && isAlive == true)   // 이전에 죽어있었는데 지금은 살아있는 경우
            {
                _visualController.TriggerSpawn();       // 비주얼 보이게 하기
            }
            // or whether it just got destroyed.
            else if (wasAlive == true && isAlive == false)  // 이전에 살아있었는데 지금 죽은 경우
            {
                _visualController.TriggerDestruction(); // 비주얼 안보이게 하기
            }
        }

        public override void FixedUpdateNetwork()
        {
            // 리스폰 체크
            if (_respawnTimer.Expired(Runner))  // 리스폰 타이머 만료 체크
            {
                _isAlive = true;            // true로 설정해서 보이게 만들기
                _respawnTimer = default;    // 리스폰 타이머 제거
            }

            // 우주선이 운석과 충돌했는지 확인
            if (_isAlive && HasHitAsteroid())   // 살아있고, 운석과 충돌했으면
            {
                ShipWasHit();   // 우주선도 맞았다고 처리
            }
        }

        // Check asteroid collision using a lag compensated OverlapSphere
        private bool HasHitAsteroid()
        {
            _lagCompensatedHits.Clear();

            var count = Runner.LagCompensation.OverlapSphere(_rigidbody.position, _spaceshipDamageRadius,
                Object.InputAuthority, _lagCompensatedHits,
                _asteroidCollisionLayer.value);

            if (count <= 0) return false;

            _lagCompensatedHits.SortDistance();

            var asteroidBehaviour = _lagCompensatedHits[0].GameObject.GetComponent<AsteroidBehaviour>();
            if (asteroidBehaviour.IsAlive == false)
                return false;

            asteroidBehaviour.HitAsteroid(PlayerRef.None);

            return true;
        }

        // Toggle the _isAlive boolean if the spaceship was hit and check whether the player has any lives left.
        // If they do, then the _respawnTimer is activated.
        private void ShipWasHit()
        {
            _isAlive = false;

            ResetShip();

            if (Object.HasStateAuthority == false) return;

            if (_playerDataNetworked.Lives > 1)
            {
                _respawnTimer = TickTimer.CreateFromSeconds(Runner, _respawnDelay);
            }
            else
            {
                _respawnTimer = default;
            }

            _playerDataNetworked.SubtractLife();

            FindObjectOfType<GameStateController>().CheckIfGameHasEnded();
        }

        // Resets the spaceships movement velocity
        private void ResetShip()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}