using System;
using Data;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface IGameManager
{
    #region Events

    event Action<IGameResult> GameResultChanged;
    event Action LevelLoaded;
    event Action<IGameResult> LevelEnded;

    #endregion
    
    Tilemap Tilemap { get; }
    LevelConfig LevelConfig { get; }
    Grid Grid { get; }
    GameStage GameStage { get; set; }
    void Restart();
    void LoadLevel(LevelConfig config);
    void NextLevel();
}

public enum GameStage
{
    Preloaded,
    Playing,
    Ended,
    Pause
}

public class GameManager : MonoBehaviour, IGameManager
{
    // Events

    public event Action LevelLoaded;
    public event Action<IGameResult> LevelEnded;
    public event Action<IGameResult> GameResultChanged;
    public event Action<IGameResult> GameEnded;

    //

    [Inject] private IGameState _gameState;
    [Inject] private IGameConfig _gameConfig;
    [Inject] private ILevelManager _levelManager;
    [Inject] private ITurnManager _turnManager;
    [Inject] private DiContainer _container;
    [Inject] private IGameProgressAccessor _gameProgressAccessor;

    public Tilemap Tilemap { get; private set; }

    [field: SerializeField] public LevelSet LevelSet { get; private set; }
    [field: SerializeField] public LevelConfig LevelConfig { get; private set; }
    [field: SerializeField] public Grid Grid { get; private set; }
    [field: SerializeField] public GameStage GameStage { get; set; } = GameStage.Preloaded;

    private void Start()
    {
        if(LevelConfig is not null && GameStage == GameStage.Preloaded)
            LoadLevel(LevelConfig);
    }

    public void Restart()
    {
        _levelManager.LoadLevel(LevelConfig);
    }

    public void NextLevel()
    {
        var presentLevelIndex = LevelSet.LevelConfigs.IndexOf(LevelConfig);

        if (presentLevelIndex == LevelSet.LevelConfigs.Count - 1)
        {
            _levelManager.LoadMainMenu();
            return;
        }
        
        var nextLevel = LevelSet.LevelConfigs[presentLevelIndex + 1];
        
        _levelManager.LoadLevel(nextLevel);
    }
    
    private void OnEnable()
    {
        _gameState.Changed += CheckForEndGameCondition;
        _turnManager.TurnStarted += CheckForEndGameCondition;
    }

    private void OnDisable()
    {
        _gameState.Changed -= CheckForEndGameCondition;
        _turnManager.TurnStarted -= CheckForEndGameCondition;
    }


    public void LoadLevel(LevelConfig config)
    {
        LevelConfig = config;
        Debug.Log("Instantiating level map...");
        Tilemap = Instantiate(config.LevelDescriptor.Map, Grid.transform, false);

        _gameState.ApplyMutation(new GameStateMutation()
        {
            PopulationChange = config.InitialPopulation,
            MoneyChange = config.InitialMoney,
            ImmigrationChange = _gameConfig.ImmigrationSettings.InitialImmigration
        });

        foreach (var building in LevelConfig.BuildingSet)
        {
            _container.Inject(building);
        }

        LevelLoaded?.Invoke();
        GameStage = GameStage.Playing;

        _gameState.Initialize();
    }

    private void CheckForEndGameCondition()
    {
        if(GameStage != GameStage.Playing)
            return;
        
        Debug.Log($"Checking end game condition... (Turn: { _gameState.Turn})");

        var result = LevelConfig.WinCondition.Check(_gameState);

        GameResultChanged?.Invoke(result);
        
        
        if (result.Won)
        {
            var progress = _gameProgressAccessor.Progress;
            
            if (progress.level < LevelSet.LevelConfigs.IndexOf(LevelConfig))
            {
                progress.level = LevelSet.LevelConfigs.IndexOf(LevelConfig);
                _gameProgressAccessor.Save();
            }

            GameStage = GameStage.Ended;
            LevelEnded?.Invoke(result);
            return;
        }

        if (result.Lost)
        {
            GameStage = GameStage.Ended;
            LevelEnded?.Invoke(result);
            return;
        }
    }
}