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
    [Inject] private DiContainer _container;

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
            PopulationChange = config.InitialPopulation,
            MoneyChange = config.InitialMoney
        });

        foreach (var building in LevelConfig.BuildingSet)
        {
            _container.Inject(building);
        }

        _gameState.Changed += CheckForEndGameCondition;
        
        LevelLoaded?.Invoke();
        
        _gameState.Initialize();
    }

    private void CheckForEndGameCondition()
    {
        var result = LevelConfig.WinCondition.Check(_gameState);

        if (result.Won)
        {
            Win();
            return;
        }

        if (result.Lost)
        {
            Lose();
            return;
        }
    }

    private void Lose()
    {
        throw new NotImplementedException();
    }

    private void Win()
    {
        throw new NotImplementedException();
    }
}