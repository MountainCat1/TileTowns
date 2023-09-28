using System;
using Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Zenject;

public interface IGameManager
{
    event Action LevelLoaded;
    Tilemap Tilemap { get; }
    LevelConfig LevelConfig { get; }
    Grid Grid { get; }
    event Action<GameResult> LevelEnded;
    void Restart();
    void LoadLevel(LevelConfig config);
}

public class GameManager : MonoBehaviour, IGameManager
{
    // Events

    public event Action LevelLoaded;
    public event Action<GameResult> LevelEnded;

    //

    [Inject] private IGameState _gameState;
    [Inject] private ILevelManager _levelManager;
    [Inject] private DiContainer _container;

    [SerializeField] private string mainMenuScene;
    [SerializeField] private string levelScene;

    public Tilemap Tilemap { get; private set; }

    [field: SerializeField] public LevelSet LevelSet { get; private set; }
    [field: SerializeField] public LevelConfig LevelConfig { get; private set; }
    [field: SerializeField] public Grid Grid { get; private set; }

    private void Start()
    {
        LoadLevel(LevelConfig);
    }

    public void Restart()
    {
        _levelManager.LoadLevel(LevelConfig);
    }

    private void OnEnable()
    {
        _gameState.Changed += CheckForEndGameCondition;
    }

    private void OnDisable()
    {
        _gameState.Changed -= CheckForEndGameCondition;
    }


    public void LoadLevel(LevelConfig config)
    {
        Debug.Log("Instantiating level map...");
        Tilemap = Instantiate(LevelConfig.LevelDescriptor.Map, Grid.transform, false);

        _gameState.Reset();

        _gameState.ApplyMutation(new GameStateMutation()
        {
            PopulationChange = config.InitialPopulation,
            MoneyChange = config.InitialMoney
        });

        foreach (var building in LevelConfig.BuildingSet)
        {
            _container.Inject(building);
        }

        LevelLoaded?.Invoke();

        _gameState.Initialize();
    }

    private void CheckForEndGameCondition()
    {
        var result = LevelConfig.WinCondition.Check(_gameState);

        if (result.Won)
        {
            LevelEnded?.Invoke(result);
            return;
        }

        if (result.Lost)
        {
            LevelEnded?.Invoke(result);
            return;
        }
    }
}