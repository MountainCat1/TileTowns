using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public interface IPopulationController
{
    public event Action<TileData> WorkerAssigned;
    public event Action<TileData> WorkerAssignedFailed;
    public event Action<TileData> WorkerUnassigned;
}

public class PopulationController : MonoBehaviour, IPopulationController
{
    #region Events

    public event Action<TileData> WorkerAssigned;
    public event Action<TileData> WorkerAssignedFailed;
    public event Action<TileData> WorkerUnassigned;

    #endregion
    
    
    [Inject] private IPlayerController _playerController;
    [Inject] private ITileMapData _tileMapData;
    [Inject] private IGameManager _gameManager;
    [Inject] private ITileSelector _tileSelector;
    [Inject] private IGameState _gameState;
    [Inject] private ITurnManager _turnManager;
    
    [SerializeField] private TilePopulationUI tilePopulationUIPrefab;
    [SerializeField] private Canvas tilePopulationUIContainer;

    private List<GameObject> _instantiatedPrefabs = new List<GameObject>();
    
    private void Start()
    {
        _playerController.PlayerModeSet += mode =>
        {
            if (mode == PlayerMode.PopulationManaging)
                DisplayPopulationUI();
            else
                HidePopulationUI();
        };

        _tileSelector.TilePointerClicked += (i, data) =>
        {
            if(_playerController.PlayerMode != PlayerMode.PopulationManaging)
                return;

            AddWorker(data);
        };
        
        _tileSelector.TilePointerSecondaryClicked += (i, data) =>
        {
            if(_playerController.PlayerMode != PlayerMode.PopulationManaging)
                return;

            RemoveWorker(data);
        };
        
        _turnManager.TurnStarted += OnTurnStarted;
    }
    
    private void OnTurnStarted()
    {
        if(_gameState.Population >= _tileMapData.AssignedWorkers)
            return;
        
        // Get random building to remove worker from it
        // to adjuct for emmigration
        var tile = _tileMapData.Data.Values
            .Where(x => x.Building is not null)
            .Where(x => x.WorkersAssigned > 0)
            .OrderBy(x => Random.Range(0f, 1f))
            .FirstOrDefault();
        
        RemoveWorker(tile);
    }

    private bool RemoveWorker(TileData tileData)
    {
        if (tileData.WorkersAssigned == 0)
            return false;
        
        WorkerUnassigned?.Invoke(tileData);
        
        return tileData.RemoveWorker();
    }

    private bool AddWorker(TileData tileData)
    {
        if (_gameState.Population <= _tileMapData.AssignedWorkers)
        {
            WorkerAssignedFailed?.Invoke(tileData);
            return false;
        }
        
        WorkerAssigned?.Invoke(tileData);
        
        var result = tileData.AddWorker();
        
        if(result == true)
            WorkerAssigned?.Invoke(tileData);
        else
            WorkerAssignedFailed?.Invoke(tileData);

        return result;
    }

    private void HidePopulationUI()
    {
        foreach (var go in _instantiatedPrefabs)
        {
            Destroy(go);
        }
        _instantiatedPrefabs.Clear();
    }

    private void DisplayPopulationUI()
    {
        foreach (var tileData in _tileMapData.TileData.Where( x => x.Building is not null))
        {
            InstantiatePopulationUI(tileData);
        }
    }

    private void InstantiatePopulationUI(TileData tileData)
    {
        var script = Instantiate(tilePopulationUIPrefab, tilePopulationUIContainer.transform);
        
        _instantiatedPrefabs.Add(script.gameObject);
        script.Initialize(tileData);

        var position = _gameManager.Grid.GetCellCenterWorld((Vector3Int)tileData.Position);

        script.transform.position = position;
    }
}