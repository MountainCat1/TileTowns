using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ISoundPlayer
{
    public void PlaySound(AudioClip clip, SoundType soundType = SoundType.Sfx);
}

public class SoundPlayer : ISoundPlayer
{
    private const float DelayToDestroyNonPlayingAudioSource = 0.5f;

    private Transform soundParent;

    private readonly Dictionary<SoundType, IList<AudioSource>> _audioSources = new();
    
    [Inject] private Camera _camera;
    [Inject]
    private void Construct()
    {
        _audioSources[SoundType.Music] = new List<AudioSource>();
        _audioSources[SoundType.Sfx] = new List<AudioSource>();
        
        soundParent = _camera.transform;
    }
    
    public void PlaySound(AudioClip clip, SoundType soundType = SoundType.Sfx)
    {
        if (clip is null)
            Debug.LogWarning($"Missing sound!");

        var audioSource = PlayAtPoint(clip, soundParent);
        _audioSources[soundType].Add(audioSource);
    }

    public void ChangeVolume(SoundType soundType, float targetVolume)
    {
        foreach (var audioSource in _audioSources[soundType])
        {
            audioSource.volume = targetVolume;
        }
    }

    public static AudioSource PlayAtPoint(AudioClip clip, Transform parent, float volume = 1f)
    {
        GameObject audioObject = new GameObject("AudioPlayer");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        if (parent != null)
        {
            audioObject.transform.SetParent(parent);
        }

        audioSource.clip = clip;
        audioSource.Play();

        Object.Destroy(audioObject, clip.length + DelayToDestroyNonPlayingAudioSource);

        return audioSource;
    }
}