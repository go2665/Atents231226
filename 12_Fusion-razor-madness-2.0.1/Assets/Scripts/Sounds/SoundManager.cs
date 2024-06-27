using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundChannelSO SFXChannel;
    public SoundChannelSO MusicChannel;
    private AudioSource _sfxSource;
    private AudioSource _musicSource;

    private void Awake()
    {
        SetupAudioSources();
        SFXChannel.SetListener(this);
        MusicChannel.SetListener(this);
    }

    private void SetupAudioSources()
    {
        _sfxSource = gameObject.AddComponent<AudioSource>();
        _musicSource = gameObject.AddComponent<AudioSource>();
        _sfxSource.playOnAwake = false;
        _musicSource.playOnAwake = false;
        _sfxSource.loop = false;
        _musicSource.loop = true;
        _musicSource.volume = .15f;
    }

    public void PlaySFX(SoundSO sound, AudioSource source = null)
    {
        sound.PlaySoundOneShot(source ? source : _sfxSource);
    }

    public void PlayMusic(SoundSO sound)
    {
        StopMusic();
        sound.PlayAsClip(_musicSource);
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
}
