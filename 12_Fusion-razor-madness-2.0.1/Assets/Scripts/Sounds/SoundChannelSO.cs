using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SoundChannel", fileName = "SoundChannel")]
public class SoundChannelSO : ScriptableObject
{
    [HideInInspector]
    public SoundManager Listener;

    public void SetListener(SoundManager listener)
    {
        Listener = listener;
    }

    public void CallSoundEvent(SoundSO sound, AudioSource source = null)
    {
        Listener?.PlaySFX(sound, source ? source : null);
    }

    public void CallMusicEvent(SoundSO sound)
    {
        Listener?.PlayMusic(sound);
    }
}
