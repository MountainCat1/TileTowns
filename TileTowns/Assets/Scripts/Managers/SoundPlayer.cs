using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum SoundType
{
    Sfx,
    Music
}

public interface ISoundPlayer
{
    public void PlaySound(AudioClip clip, SoundType soundType = SoundType.Sfx);
}

public class SoundPlayer : ISoundPlayer
{
    private const float DelayToDestroyNonPlayingAudioSource = 0.5f;

    private Transform soundParent;

    private readonly Dictionary<SoundType, IList<AudioSource>> _audioSources = new();
    private readonly Dictionary<SoundType, float> _volumes = new();

    [Inject] private Camera _camera;
    [Inject]
    private void Construct()
    {
        _audioSources[SoundType.Music] = new List<AudioSource>();
        _audioSources[SoundType.Sfx] = new List<AudioSource>();
        
        // TODO: Load from settings
        _volumes[SoundType.Music] = 0.5f;
        _volumes[SoundType.Sfx] = 0.5f;
        
        soundParent = _camera.transform;
    }
    
    public void PlaySound(AudioClip clip, SoundType soundType = SoundType.Sfx)
    {
        if (clip is null)
            Debug.LogWarning($"Missing sound!");

        var volume = _volumes[soundType];
        
        var audioSource = PlayAtPoint(clip, soundParent, volume);
        _audioSources[soundType].Add(audioSource);
    }

    public void ChangeVolume(SoundType soundType, float targetVolume)
    {
        _volumes[soundType] = targetVolume;
        foreach (var audioSource in _audioSources[soundType])
        {
            audioSource.volume = targetVolume;
        }
    }

    public static AudioSource PlayAtPoint(AudioClip clip, Transform parent, float volume = 1f, bool destroy = true)
    {
        GameObject audioObject = new GameObject("AudioPlayer");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        if (parent != null)
        {
            audioObject.transform.SetParent(parent);
        }

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        if (destroy)
            Object.Destroy(audioObject, clip.length + DelayToDestroyNonPlayingAudioSource);

        return audioSource;
    }
}