using System.Collections.Generic;
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
    [Inject] private IGameSounds _gameSounds;
    [Inject] private ISoundPlayer _soundPlayer;
    
    [Inject] private IBuildingController _buildingController;
    [Inject] private ITurnManager _turnManager;
    [Inject] private IGameManager _gameManager;
    [Inject] private IPopulationController _populationController;

    [Inject]
    private void Construct()
    {

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
        
        _soundPlayer.PlaySound(clip, soundType);
    }
}