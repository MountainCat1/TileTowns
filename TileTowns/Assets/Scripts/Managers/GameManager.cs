using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface IGameManager
{
    event Action LevelLoaded;
    Tilemap Tilemap { get; }
    LevelConfig LevelConfig { get; }
    Grid Grid { get; }
}

public class GameManager : MonoBehaviour, IGameManager
{
    // Events

    public event Action LevelLoaded;
    
    //

    [Inject] private IGameState _gameState;
    
    public Tilemap Tilemap { get; private set; }

    [field: SerializeField] public LevelConfig LevelConfig { get; private set; }
    [field: SerializeField] public Grid Grid { get; private set; }

    private void Start()
    {
        LoadLevel(LevelConfig);
    }

    public void LoadLevel(LevelConfig config)
    {
        Debug.Log("Instantiating level map...");
        Tilemap = Instantiate(LevelConfig.LevelDescriptor.Map, Grid.transform, false);

        _gameState.ApplyMutation(new GameStateMutation()
        {
            PopulationChange = config.InitialPopulation
        });
        
        LevelLoaded?.Invoke();
    }
}
