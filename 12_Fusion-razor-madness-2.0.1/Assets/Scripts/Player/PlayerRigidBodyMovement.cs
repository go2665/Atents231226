using UnityEngine;
using Fusion;
using NetworkRigidbody2D = Fusion.Addons.Physics.NetworkRigidbody2D;

public class PlayerRigidBodyMovement : NetworkBehaviour
{
    [Header("Movement")]
    // 플레이어 비헤이비어(입력이 허용되어 있는지 확인용)
    private PlayerBehaviour _behaviour;
    // 바닥 레이어
    [SerializeField] private LayerMask _groundLayer;
    // 리지드바디
    private NetworkRigidbody2D _rb;
    // 입력 받는 클래스
    private InputController _inputController;

    // 이동 속도
    [SerializeField] float _speed = 10f;
    // 점프력
    [SerializeField] float _jumpForce = 10f;
    // 최대 이동 속도(정확히 이 값으로 최대치가 설정되는 것은 아님. 근사치로 설정됨)
    [SerializeField] float _maxVelocity = 8f;

    // 일반적으로 떨어질 때의 보정값
    [SerializeField] private float fallMultiplier = 3.3f;
    // 낮은 점프용 보정값
    [SerializeField] private float lowJumpMultiplier = 2f;
    // 벽 슬라이딩 중일 때 떨어지는 보정 값
    private readonly float wallSlidingMultiplier = 1f;

    // 바닥 수평 마찰력
    private Vector2 _groundHorizontalDragVector = new Vector2(.1f, 1);      // 90%감소
    // 공중 수평 마찰력
    private Vector2 _airHorizontalDragVector = new Vector2(.98f, 1);        // 2%감소
    // 최대치를 넘어섰을 때 감쇄용(수평)
    private Vector2 _horizontalSpeedReduceVector = new Vector2(.95f, 1);
    // 최대치를 넘어섰을 때 감쇄용(수직)
    private Vector2 _verticalSpeedReduceVector = new Vector2(1, .95f);

    // 컬라이더
    private Collider2D _collider;
    // 땅에 닿았으면 true, 아니면 false
    [Networked]
    private NetworkBool IsGrounded { get; set; }

    // 벽에 닿아있으면 true, 아니면 false
    private bool _wallSliding;
    // 벽점프용 노멀벡터. 벽이 왼쪽에 있으면 오른쪽, 벽이 오른쪽에 있으면 왼쪽을 가리킨다.
    private Vector2 _wallSlidingNormal;

    // 긴 점프용?
    private float _jumpBufferThreshold = .2f;
    // 공중에 있을 때 점프버튼을 누르고 있는 시간?
    private float _jumpBufferTime;

    // 코요테타임(발판이 없어도 일정 시간동안 점프 가능하게 하는 것)용 임계값
    [Networked]
    private float CoyoteTimeThreshold { get; set; } = .1f;

    // 땅에서 떨어지고 나서 지난 시간(점프 말고 바닥이 없는데 앞으로 간 경우의 시간)
    [Networked]
    private float TimeLeftGrounded { get; set; }

    // 코요테타임 중인지 아닌지? 점프로 바닥에서 떨어졌는지 아니면 절벽에서 떨어졌는지 체크용?
    [Networked]
    private NetworkBool CoyoteTimeCD { get; set; }

    // 이전 물리프레임에 땅에 닿아있는지 여부
    [Networked]
    private NetworkBool WasGrounded { get; set; }

    // Rigidbody의 벨로시티(참조용)
    [Networked] public Vector3 Velocity { get; set; }

    [Space()]
    [Header("Particle")]
    // 파티클을 풀로 관리하는 스크립터블 오브젝트 클래스
    [SerializeField] private ParticleManager _particleManager;

    [Space()]
    [Header("Sound")]
    // 사운드 관련
    [SerializeField] private SoundChannelSO _sfxChannel;
    [SerializeField] private SoundSO _jumpSound;
    [SerializeField] private AudioSource _playerSource;

    void Awake()
    {
        // 컴포넌트 찾기
        _rb = GetComponent<NetworkRigidbody2D>();
        _collider = GetComponentInChildren<Collider2D>();
        _behaviour = GetBehaviour<PlayerBehaviour>();
        _inputController = GetBehaviour<InputController>();
    }

    public override void Spawned()
    {
        // Object.InputAuthority 플레이어가 Object를 항상 관심있게 보고 있다.(리플리케이트 될 필요가 없음. 클라이언트측에서 예측을 하기 때문에?) 
        Runner.SetPlayerAlwaysInterested(Object.InputAuthority, Object, true);
    }

    /// <summary>
    /// 바닥에 닿았거나 벽에 닿은 상태인지 확인하는 함수(물리 프레임 업데이트마다 실행됨)
    /// </summary>
    private void DetectGroundAndWalls()
    {
        WasGrounded = IsGrounded;   // 이전에 바닥에 닿았는지를 기록
        IsGrounded = default;       // 초기화
        _wallSliding = default;

        // 지금 바닥에 닿았는지를 체크(캐릭터 컬라이더에서 바닥쪽 약간만 나가는 영역으로 체크)
        IsGrounded = (bool)Runner.GetPhysicsScene2D().OverlapBox(
            (Vector2)transform.position + Vector2.down * (_collider.bounds.extents.y - .3f), // 오버랩할 박스의 위치 구하기
            Vector2.one * .85f,     // 오버랩 박스의 크기 결정
            0,                      // 회전은 없음
            _groundLayer);          // 땅 레이어와만 체크

        // 바닥에 닿았으면
        if (IsGrounded)
        {
            CoyoteTimeCD = false;   // CoyoteTimeCD를 false로 설정하고
            return;                 // 함수 종료(바닥 체크가 충돌용 컬라이더 보다 작기 때문에 바닥에 닿았다고 체크되면 벽에 닿을일은 없음)
        }

        // 이전 프레임에 바닥에 닿았다면
        if (WasGrounded)
        {
            if (CoyoteTimeCD)       // 점프를 하면 CoyoteTimeCD는 true가 된다.
            {
                CoyoteTimeCD = false;
            }
            else
            {
                TimeLeftGrounded = Runner.SimulationTime;   // 마지막 FixedUpdateNetwork에서 얼마나 시간이 지났는지를 기록
            }
        }

        // 오른쪽에 벽이 있는지 확인
        _wallSliding = Runner.GetPhysicsScene2D().OverlapCircle(
            transform.position + Vector3.right * (_collider.bounds.extents.x),  // 원의 위치(컬라이더의 오른쪽 면)
            .1f,            // 원의 반지름
            _groundLayer);  // 확인할 레이어
        if (_wallSliding)
        {
            // 오른쪽에 벽이 있다.
            _wallSlidingNormal = Vector2.left;  // 벽 점프용 노멀값 설정
            return;
        }
        else
        {
            // 왼쪽에 벽이 있는지 확인
            _wallSliding = Runner.GetPhysicsScene2D().OverlapCircle(transform.position - Vector3.right * (_collider.bounds.extents.x), .1f, _groundLayer);
            if (_wallSliding)
            {
                // 왼쪽에 벽이 있다.
                _wallSlidingNormal = Vector2.right; // 벽 점프용 노멀값 설정
            }
        }

    }

    // 바닥에 닿았는지 리턴하는 함수
    public bool GetGrounded()
    {
        return IsGrounded;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<InputData>(out var input)) // 입력 값 받아오기
        {
            var pressed = input.GetButtonPressed(_inputController.PrevButtons); // PrevButtons(이전 버튼 상태)를 임시로 저장하고
            _inputController.PrevButtons = input.Buttons;   // 새 버튼 상태를 PrevButtons에 기록
            UpdateMovement(input);      // 현재 버튼 상태를 기반으로 움직임처리
            Jump(pressed);              // 이전 버튼 상태를 기반으로 점프 처리
            BetterJumpLogic(input);     // 현재 번튼 상태를 기반으로 더 좋은 점프로직 실행
        }

        Velocity = _rb.Rigidbody.velocity;  // 외부 참조용 Velocity에 리지드바디의 벨로시티 저장
    }

    // 이동 처리용 함수
    void UpdateMovement(InputData input)
    {
        DetectGroundAndWalls(); // 바닥에 닿았는지 확인

        if (input.GetButton(InputButton.LEFT) && _behaviour.InputsAllowed)  // 입력이 허용된 상황에서 왼쪽 누르기
        {
            if (_rb.Rigidbody.velocity.x > 0 && IsGrounded) // 바닥에 있는 상태에서 반대방향으로 움직이고 있으면
            {
                _rb.Rigidbody.velocity *= Vector2.up;       // 벨로시티의 x값 0으로 만들기
            }
            _rb.Rigidbody.AddForce(Vector2.left * _speed * Runner.DeltaTime, ForceMode2D.Force);    // 입력된 방향으로 힘 추가
        }
        else if (input.GetButton(InputButton.RIGHT) && _behaviour.InputsAllowed)    // 입력이 허용된 상황에서 오른쪽 누르기
        {
            if (_rb.Rigidbody.velocity.x < 0 && IsGrounded) // 왼쪽이랑 동일
            {
                _rb.Rigidbody.velocity *= Vector2.up;
            }
            _rb.Rigidbody.AddForce(Vector2.right * _speed * Runner.DeltaTime, ForceMode2D.Force);
        }
        else // 입력이 허용되어 있지 않거나, 입력이 허용되었더라도 좌우 중 눌려진게 없는 경우
        {
            // 바닥에 있는지 공중에 있는지에 따라 적절한 마찰력 적용하기
            if (IsGrounded)
                _rb.Rigidbody.velocity *= _groundHorizontalDragVector;
            else
                _rb.Rigidbody.velocity *= _airHorizontalDragVector;
        }

        LimitSpeed();   // 속도 제한 걸기
    }

    // 속도 제한거는 함수
    private void LimitSpeed()
    {
        //Limit horizontal velocity
        if (Mathf.Abs(_rb.Rigidbody.velocity.x) > _maxVelocity)
        {
            _rb.Rigidbody.velocity *= _horizontalSpeedReduceVector; // x에 0.95를 곱해서 살짝 줄이기
        }

        if (Mathf.Abs(_rb.Rigidbody.velocity.y) > _maxVelocity * 2)
        {
            _rb.Rigidbody.velocity *= _verticalSpeedReduceVector;   // y에 0.95를 곱해서 살짝 줄이기
        }
    }

    #region Jump
    // 점프 처리용 함수(pressedButtons:이전 프레임의 입력)
    private void Jump(NetworkButtons pressedButtons)
    {
        // 매 물리 플레임마다 실행
        if (pressedButtons.IsSet(InputButton.JUMP) || CalculateJumpBuffer())    // 이전 프레임에 점프버튼이 눌려져 있었거나 점프버퍼의 계산결과 true일 때
        {
            if (_behaviour.InputsAllowed)   // 입력이 허용되어 있고
            {
                if (!IsGrounded && pressedButtons.IsSet(InputButton.JUMP))  // 공중에 있으면서 이전에 점프 버튼을 누르고 있으면
                {
                    _jumpBufferTime = Runner.SimulationTime;    // 이전 프레임에서 얼마나 지났는지를 기록(긴점프용?)
                }
                                
                if (IsGrounded || CalculateCoyoteTime())        // 땅에 닿아있거나 코요테타임 중이면
                {
                    _rb.Rigidbody.velocity *= Vector2.right;    // 벨로시티의 y만 0으로 만들기
                    _rb.Rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);   // 위로 힘을 더하기
                    CoyoteTimeCD = true;                        // CoyoteTimeCD를 true로 세팅
                    if (Runner.IsForward && Object.HasInputAuthority)   // 처음 계산되는 경우고, 입력 권한을 가지고 있을 경우
                    {
                        RPC_PlayJumpEffects((Vector2)transform.position - Vector2.up * .5f);    // 점프 사운드와 파티클 재생
                    }
                }
                else if (_wallSliding)  // 벽에 닿아있는 상태면
                {
                    _rb.Rigidbody.velocity *= Vector2.zero; // 벨로시티 초기화
                    _rb.Rigidbody.AddForce((Vector2.up + (_wallSlidingNormal)) * _jumpForce, ForceMode2D.Impulse);  // 대각선 위로 점프
                    CoyoteTimeCD = true;                    // CoyoteTimeCD를 true로 세팅
                    if (Runner.IsForward && Object.HasInputAuthority)   // 처음 계산되는 경우고, 입력 권한을 가지고 있을 경우
                    {
                        RPC_PlayJumpEffects((Vector2)transform.position - _wallSlidingNormal * .5f);    // 점프 사운드와 파티클 재생
                    }
                }
            }
        }
    }

    // 점프 버퍼를 계산하는 함수
    private bool CalculateJumpBuffer()
    {
        // Runner.SimulationTime : 마지막 FixedUpdateNetwork에서 지난 시간
        return (Runner.SimulationTime <= _jumpBufferTime + _jumpBufferThreshold) && IsGrounded;
    }

    // 코요테 타임을 계산하기 위한 함수
    private bool CalculateCoyoteTime()
    {
        // 시간이 임계값 안쪽이다?
        return (Runner.SimulationTime <= TimeLeftGrounded + CoyoteTimeThreshold);   
    }

    // 점프 소리와 이팩트를 생성하는 요청
    [Rpc(sources: RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_PlayJumpEffects(Vector2 particlePos)
    {
        PlayJumpSound();
        PlayJumpParticle(particlePos);
    }

    private void PlayJumpSound()
    {
        _sfxChannel.CallSoundEvent(_jumpSound, Object.HasInputAuthority ? null : _playerSource);
    }

    private void PlayJumpParticle(Vector2 pos)
    {
        _particleManager.Get(ParticleManager.ParticleID.Jump).transform.position = pos;
    }

    /// <summary>
    /// Increases gravity force on the player based on input and current fall progress.
    /// </summary>
    /// <param name="input"></param>
    private void BetterJumpLogic(InputData input)
    {
        if (IsGrounded) { return; }         // 바닥에 있는 상황이면 종료(= 공중이고)
        if (_rb.Rigidbody.velocity.y < 0)   
        {
            // 떨어지는 상황이다.
            if (_wallSliding && input.AxisPressed())
            {
                // 벽에서 좌우 중 입력하고 있을 때(벽슬라이딩 중)
                _rb.Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (wallSlidingMultiplier - 1) * Runner.DeltaTime;
            }
            else
            {
                // 벽이 아니거나 입력이 없거나(벽슬라이딩이 아닌 경우)
                _rb.Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Runner.DeltaTime;
            }
        }
        else if (_rb.Rigidbody.velocity.y > 0 && !input.GetButton(InputButton.JUMP))
        {
            // 위로 점프 중일 때 점프버튼이 안눌러져 있는 경우(낮은 점프 처리)
            _rb.Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Runner.DeltaTime;
        }
    }
    #endregion
}
