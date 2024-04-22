using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class NetPlayerDecorator : NetworkBehaviour
{
    // 몸 색 --------------------------------------------------------------------------------
    NetworkVariable<Color> bodyColor = new NetworkVariable<Color>();

    Renderer playerRenderer;
    Material bodyMaterial;

    readonly int BaseColor_Hash = Shader.PropertyToID("_BaseColor");

    // 이름 --------------------------------------------------------------------------------------
    NetworkVariable<FixedString32Bytes> userName = new NetworkVariable<FixedString32Bytes>();
    NamePlate namePlate;


    // 이팩트 -------------------------------------------------------------------------------------
    NetworkVariable<bool> netEffectState = new NetworkVariable<bool>(false);
    public bool IsEffectOn
    {
        get => netEffectState.Value;
        set
        {
            if (netEffectState.Value != value)
            {
                if (IsServer)
                {
                    netEffectState.Value = value;
                }
                else
                {
                    UpdateEffectStateServerRpc(value);
                }
            }
        }
    }
    readonly int EmissionIntensity_Hash = Shader.PropertyToID("_EmissionIntensity");


    private void Awake()
    {
        playerRenderer = GetComponentInChildren<Renderer>();
        bodyMaterial = playerRenderer.material;

        bodyColor.OnValueChanged += OnBodyColorChange;


        namePlate = GetComponentInChildren<NamePlate>();
        userName.OnValueChanged += OnNameSet;


        netEffectState.OnValueChanged += OnEffectStateChange;
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            bodyColor.Value = UnityEngine.Random.ColorHSV(0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f);
        }
        bodyMaterial.SetColor(BaseColor_Hash, bodyColor.Value);
    }

    // 이름 설정용 -------------------------------------------------------------------------------------
    public void SetName(string name)
    {
        if(IsOwner)
        {
            if(IsServer)
            {
                userName.Value = name;
            }
            else
            {
                RequestUserNameChangeServerRpc(name);
            }
        }
    }

    [ServerRpc]
    void RequestUserNameChangeServerRpc(string name)
    {
        userName.Value = name;
    }

    private void OnNameSet(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        namePlate.SetName(newValue.ToString());
    }

    public void RefreshNamePlate()
    {
        namePlate.SetName(userName.Value.ToString());
    }

    // 색상 설정용 -------------------------------------------------------------------------------------------------

    public void SetColor(Color color)
    {
        if(IsOwner)
        {
            if(IsServer)
            {
                bodyColor.Value = color;
            }
            else
            {
                RequestBodyColorChangeServerRpc(color);
            }
        }
    }

    [ServerRpc]
    void RequestBodyColorChangeServerRpc(Color color)
    {
        bodyColor.Value = color;
    }

    private void OnBodyColorChange(Color previousValue, Color newValue)
    {
        bodyMaterial.SetColor(BaseColor_Hash, newValue);
    }


    // 이팩트용 ------------------------------------------------------------------------------


    private void OnEffectStateChange(bool previousValue, bool newValue)
    {
        if(newValue)
        {
            bodyMaterial.SetFloat(EmissionIntensity_Hash, 1.0f);
        }
        else
        {
            bodyMaterial.SetFloat(EmissionIntensity_Hash, 0.0f);
        }
    }

    [ServerRpc]
    void UpdateEffectStateServerRpc(bool isOn)
    {
        netEffectState.Value = isOn;
    }
}
