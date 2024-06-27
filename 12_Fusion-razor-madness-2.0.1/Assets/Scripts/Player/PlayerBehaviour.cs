using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerBehaviour : NetworkBehaviour
{
    public Transform CameraTransform;

    [Networked]
    public NetworkString<_16> Nickname { get; set; }
    [Networked]
    public Color PlayerColor { get; set; }

    [Networked]
    public int PlayerID { get; private set; }

    private Fusion.Addons.Physics.NetworkRigidbody2D _rb;
    private InputController _inputController;
    private Collider2D _collider;
    private Collider2D _hitCollider;

    [Networked]
    private TickTimer RespawnTimer { get; set; }
    [Networked]
    private NetworkBool Respawning { get; set; }
    [Networked]
    private NetworkBool Finished { get; set; }
    [Networked]
    public NetworkBool InputsAllowed { get; set; }


    [SerializeField] private ParticleManager _particleManager;

    [Space()]
    [Header("Sound")]
    [SerializeField] private SoundChannelSO _sfxChannel;
    [SerializeField] private SoundSO _deathSound;
    [SerializeField] private AudioSource _playerSource;

    private ChangeDetector _changeDetector;

    private void Awake()
    {
        _inputController = GetBehaviour<InputController>();
        _rb = GetBehaviour<Fusion.Addons.Physics.NetworkRigidbody2D>();
        _collider = GetComponentInChildren<Collider2D>();
    }

    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState, false);
        
        PlayerID = Object.InputAuthority.PlayerId;

        if (Object.HasInputAuthority)
        {
            CameraManager camera = FindObjectOfType<CameraManager>();
            camera.CameraTarget = CameraTransform;

            if (Nickname == string.Empty)
            {
                RPC_SetNickname(PlayerPrefs.GetString("Nick"));
            }
            GetComponentInChildren<SpriteRenderer>().sortingOrder += 1;
        }
        GetComponentInChildren<NicknameText>().SetupNick(Nickname.ToString());
        GetComponentInChildren<SpriteRenderer>().color = PlayerColor;
        _particleManager.ClearParticles();
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetNickname(string nick)
    {
        Nickname = nick;
    }

    public void SetInputsAllowed(bool value)
    {
        InputsAllowed = value;
    }

    private void SetRespawning()
    {
        if (Runner.IsServer)
        {
            RPC_DeathEffects();
            _rb.Rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_DeathEffects()
    {
        _particleManager.Get(ParticleManager.ParticleID.Death).transform.position = transform.position;
        PlayDeathSound();
    }

    private void SetGFXActive(bool value)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(value);
    }
 
    private void OnNickChanged()
    {
        GetComponentInChildren<NicknameText>().SetupNick(Nickname.ToString());
    }

    public override void FixedUpdateNetwork()
    {

        DetectCollisions();

        if (GetInput<InputData>(out var input) && InputsAllowed)
        {
            if (input.GetButtonPressed(_inputController.PrevButtons).IsSet(InputButton.RESPAWN) && !Respawning)
            {
                RequestRespawn();
            }
        }

        if (Respawning)
        {
            if (RespawnTimer.Expired(Runner))
            {
                _rb.Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                StartCoroutine(Respawn());
            }
        }

    }

    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(Respawning):
                    SetGFXActive(!Respawning);
                    break;
                case nameof(Nickname):
                    OnNickChanged();
                    break;
            }
        }
    }

    private void PlayDeathSound()
    {
        _sfxChannel.CallSoundEvent(_deathSound, Object.HasInputAuthority ? null : _playerSource);
    }

    private IEnumerator Respawn()
    {
        _rb.Teleport(PlayerSpawner.PlayerSpawnPos);
        yield return new WaitForSeconds(.1f);
        Respawning = false;
        SetInputsAllowed(true);
    }

    private void FinishRace()
    {
        if (Finished) { return; }

        if (Object.HasInputAuthority)
        {
            GameManager.Instance.SetPlayerSpectating(this);
        }

        if (Runner.IsServer)
        {
            FindObjectOfType<LevelBehaviour>().PlayerOnFinishLine(Object.InputAuthority, this);
            Finished = true;
        }
    }

    public void RequestRespawn()
    {
        Respawning = true;
        SetInputsAllowed(false);
        RespawnTimer = TickTimer.CreateFromSeconds(Runner, 1f);
        SetRespawning();
    }

    private void DetectCollisions()
    {
        _hitCollider = Runner.GetPhysicsScene2D().OverlapBox(transform.position, _collider.bounds.size * .9f, 0, LayerMask.GetMask("Interact"));
        if (_hitCollider != default)
        {
            if (_hitCollider.tag.Equals("Kill") && !Respawning)
            {
                RequestRespawn();
            }
            else if (_hitCollider.tag.Equals("Finish") && !Finished)
            {
                FinishRace();
            }
        }
    }
}
