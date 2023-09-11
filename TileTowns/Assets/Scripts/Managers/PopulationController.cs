using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using Zenject;

public class PopulationController : MonoBehaviour
{
    [Inject] private IPlayerController _playerController;
    [Inject] private ITileMapData _tileMapData;
    [Inject] private IGameManager _gameManager;
    [Inject] private ITileSelector _tileSelector;
    [Inject] private IGameState _gameState;
    
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
    }
    

    private bool RemoveWorker(TileData tileData)
    {
        if (tileData.WorkersAssigned == 0)
            return false;
        
        return tileData.RemoveWorker();
    }

    private bool AddWorker(TileData tileData)
    {
        if (_gameState.Population <= _tileMapData.AssignedWorkers)
            return false;
        
        return tileData.AddWorker();
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

        var position = _gameManager.Grid.GetCellCenterWorld(tileData.Position);

        script.transform.position = position;
    }
}