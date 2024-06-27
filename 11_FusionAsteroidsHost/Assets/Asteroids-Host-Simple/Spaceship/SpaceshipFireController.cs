using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using NetworkTransform = Fusion.NetworkTransform;

namespace Asteroids.HostSimple
{
    // 우주선의 발사 제어 전용 클래스
    public class SpaceshipFireController : NetworkBehaviour
    {
        // Game Session AGNOSTIC Settings
        // 총알 발사 딜레이
        [SerializeField] private float _delayBetweenShots = 0.2f;
        // 총알 프리팹
        [SerializeField] private NetworkPrefabRef _bullet = NetworkPrefabRef.Empty;

        // Local Runtime references
        // 리지드바디
        private Rigidbody _rigidbody = null;

        // 우주선 생명주기 컨트롤러(살아있는지 체크용)
        private SpaceshipController _spaceshipController = null;

        // Game Session SPECIFIC Settings
        // 입력에서 버튼들의 이전 상태(누르고 있을 때 연사되는 것 방지용)
        [Networked] private NetworkButtons _buttonsPrevious { get; set; }

        // 총알 쿨다운용 틱타이머
        [Networked] private TickTimer _shootCooldown { get; set; }

        public override void Spawned()
        {
            // --- Host & Client
            // 찾을 것들 찾아놓기
            _rigidbody = GetComponent<Rigidbody>();
            _spaceshipController = GetComponent<SpaceshipController>();
        }

        public override void FixedUpdateNetwork()
        {
            if (_spaceshipController.AcceptInput == false) return;  // 리스폰 중이나 게임오버가 된 상황이면 리턴

            if (GetInput<SpaceshipInput>(out var input) == false) return;   // 입력을 못받아오는 상황이면 리턴

            Fire(input);    // 발사처리
        }

        // 발사 처리
        private void Fire(SpaceshipInput input)
        {
            // 버튼의 이전 상태와 비교해서 방금 눌려진 상황인지 확인
            if (input.Buttons.WasPressed(_buttonsPrevious, SpaceshipButtons.Fire))  // 지금 눌려진것인지 체크
            {
                SpawnBullet();  // 총알 생성 시도
            }
            _buttonsPrevious = input.Buttons;   // 버튼 상태 저장하기
        }

        // 총알 스폰(총알은 플레이어의 앞쪽으로 날아감)
        private void SpawnBullet()
        {
            // 쿨다운이 다 되지 않았거나 러너가 스폰을 못하는 상황이면 리턴
            if (_shootCooldown.ExpiredOrNotRunning(Runner) == false || !Runner.CanSpawn) return;    

            // 스폰(총알, 내위치, 내회전, 내 플레이어레퍼런스)
            Runner.Spawn(_bullet, _rigidbody.position, _rigidbody.rotation, Object.InputAuthority);

            _shootCooldown = TickTimer.CreateFromSeconds(Runner, _delayBetweenShots);   // 발사 쿨다운 설정
        }
    }
}