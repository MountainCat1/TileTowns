﻿using UnityEngine;
using Zenject;

public interface ISoundManager
{
}

public class SoundManager : ISoundManager
{
    [Inject] private IGameSounds _gameSounds;
    [Inject] private ISoundPlayer _soundPlayer;
    
    [Inject] private IBuildingController _buildingController;
    [Inject] private IGameState _gameState;
    [Inject] private ITurnManager _turnManager;
    [Inject] private IGameManager _gameManager;
    [Inject] private IPopulationController _populationController;
    [Inject] private IGameSettingsAccessor _gameSettingsAccessor;
    
    [Inject] private Camera _camera;
     
    private AudioSource _soundtrackAudioSource;

    [Inject]
    private void Construct()
    {
        _buildingController.PlaceBuildingFailed += delegate { PlaySound(_gameSounds.Error); };
        _buildingController.BuiltBuilding += delegate { PlaySound(_gameSounds.Building); };
        
        _turnManager.TurnEnded += delegate { PlaySound(_gameSounds.TurnEnded); };
        
        _gameManager.LevelLoaded += delegate { SetSoundtrack(_gameManager.LevelConfig.Soundtrack); };
        
        _populationController.WorkerAssigned += delegate { PlaySound(_gameSounds.WorkerAssigned); };
        _populationController.WorkerUnassigned += delegate { PlaySound(_gameSounds.WorkerUndassigned); };
        _populationController.WorkerAssignedFailed += delegate { PlaySound(_gameSounds.Error); };

        _gameManager.LevelEnded += result =>
        {
            if (result.Won)
            {
                SetSoundtrack(_gameSounds.WinMusic);
            }
            else if(result.Lost)
            {
                SetSoundtrack(_gameSounds.LoseMusic);
            }
        };
        
        _gameState.PopImmigrated  += delegate { PlaySound(_gameSounds.PopImmigrated); };
        _gameState.PopEmmigrated  += delegate { PlaySound(_gameSounds.PopEmmigrated); };
    }

    public void SetSoundtrack(AudioClip clip)
    {
        if (clip is null)
            Debug.LogWarning($"Missing sound!");

        // Destroy previous soundtrack player
        Object.Destroy(_soundtrackAudioSource);
        
        _soundtrackAudioSource = _soundPlayer.CreateSound(
            clip: clip,
            soundType: SoundType.Music,
            parent: _camera.transform,
            destroy: false
        );

        _soundtrackAudioSource.loop = true;
    }
    
    private void PlaySound(AudioClip clip, SoundType soundType = SoundType.Sfx)
    {
        if (clip is null)
            Debug.LogWarning($"Missing sound!");
        
        _soundPlayer.PlaySound(clip, soundType);
    }
}