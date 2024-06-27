using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound/SoundClip", fileName = "SoundSO")]
public class SoundSO : ScriptableObject
{
    public List<AudioClip> SoundVariations;

    public void PlaySoundOneShot(AudioSource source)
    {
        if (SoundVariations.Count <= 0) { return; }
        source.PlayOneShot(SoundVariations[Random.Range(0, SoundVariations.Count)]);
    }

    public void PlayAsClip(AudioSource source)
    {
        if (SoundVariations.Count <= 0) { return; }
        source.clip = SoundVariations[Random.Range(0, SoundVariations.Count)];
        source.Play();
    }
}
