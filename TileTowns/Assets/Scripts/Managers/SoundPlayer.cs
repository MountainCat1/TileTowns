using System.Collections.Generic;
using UnityEngine;
using Zenject;

public enum SoundType
{
    Sfx,
    Music,
    UI
}

public interface ISoundPlayer
{
    public void PlaySound(AudioClip clip, SoundType soundType = SoundType.Sfx);
    AudioSource CreateSound(AudioClip clip, SoundType soundType, Transform parent, bool destroy = true);
}

public class SoundPlayer : ISoundPlayer
{
    private const float DelayToDestroyNonPlayingAudioSource = 0.5f;

    private Transform soundParent;

    // TODO - use pool instead of dictionary because it's faster
    private readonly Dictionary<SoundType, IList<AudioSource>> _audioSources = new();
    private readonly Dictionary<SoundType, float> _volumes = new();

    [Inject] private Camera _camera;
    [Inject] private IGameSettingsAccessor _settingsAccessor;
    
    [Inject]
    private void Construct()
    {
        _audioSources[SoundType.Music] = new List<AudioSource>();
        _audioSources[SoundType.Sfx] = new List<AudioSource>();
        _audioSources[SoundType.UI] = new List<AudioSource>();
        
        _volumes[SoundType.Music] = _settingsAccessor.Settings.muiscVolume;
        _volumes[SoundType.Sfx] = _settingsAccessor.Settings.sfxVolume;
        _volumes[SoundType.UI] = _settingsAccessor.Settings.uiVolume;
        
        soundParent = _camera.transform;

        _settingsAccessor.Changed += ApplyVolumeChange;
    }

    private void ApplyVolumeChange(GameSettings gameSettings)
    {
        ChangeVolume(SoundType.Music, gameSettings.muiscVolume);
        ChangeVolume(SoundType.Sfx, gameSettings.sfxVolume);
        ChangeVolume(SoundType.UI, gameSettings.uiVolume);
    }

    public void PlaySound(AudioClip clip, SoundType soundType = SoundType.Sfx)
    {
        if (clip is null)
            Debug.LogWarning($"Missing sound!");

        CreateSound(clip, soundType, soundParent);
    }

    public void ChangeVolume(SoundType soundType, float targetVolume)
    {
        _volumes[soundType] = targetVolume;
        foreach (var audioSource in _audioSources[soundType])
        {
            audioSource.volume = targetVolume;
        }
    }

    public AudioSource CreateSound(AudioClip clip, SoundType soundType, Transform parent, bool destroy = true)
    {
        GameObject audioObject = new GameObject("AudioPlayer");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        if (parent != null)
        {
            audioObject.transform.SetParent(parent);
        }

        var volume = _volumes[soundType];

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        _audioSources[soundType].Add(audioSource);
        
        if (destroy)
            Object.Destroy(audioObject, clip.length + DelayToDestroyNonPlayingAudioSource);

        return audioSource;
    }
}