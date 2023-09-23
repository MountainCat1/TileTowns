﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using UI;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public enum SoundType
{
    Sfx,
    Music
}

public interface ISoundManager
{
}

public class SoundManager : ISoundManager
{
    private const float DelayToDestroyNonPlayingAudioSource = 0.5f;
    
    [Inject] private IGameSounds _gameSounds;
    
    [Inject] private IBuildingController _buildingController;
    [Inject] private ITurnManager _turnManager;
    [Inject] private IGameManager _gameManager;
    [Inject] private IPopulationController _populationController;
    
    [Inject] private Camera _camera;

    private Transform soundParent;
    
    private readonly Dictionary<SoundType, IList<AudioSource>> _audioSources = new();

    [Inject]
    private void Construct()
    {
        _audioSources[SoundType.Music] = new List<AudioSource>();
        _audioSources[SoundType.Sfx] = new List<AudioSource>();
        
        soundParent = _camera.transform;
        
        _buildingController.PlaceBuildingFailed += delegate { PlaySound(_gameSounds.Error); };
        _buildingController.PlacedBuilding += delegate { PlaySound(_gameSounds.Building); };
        
        _turnManager.TurnEnded += delegate { PlaySound(_gameSounds.TurnEnded); };
        
        _gameManager.LevelLoaded += delegate { PlaySound(_gameSounds.GameMusic, SoundType.Music); };
        
        _populationController.WorkerAssigned += delegate { PlaySound(_gameSounds.WorkerAssigned); };
        _populationController.WorkerUnassigned += delegate { PlaySound(_gameSounds.WorkerUndassigned); };
        _populationController.WorkerAssignedFailed += delegate { PlaySound(_gameSounds.Error); };
    }

    private void PlaySound(AudioClip clip, SoundType soundType = SoundType.Sfx)
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