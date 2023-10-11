using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class BuildingGhost : MonoBehaviour
{
    [Inject] private IBuildingController _buildingController;
    [Inject] private ITileSelector _tileSelector;
    [Inject] private ITileMapData _tileMapData;
    [Inject] private IPlayerController _playerController;
    [Inject] private IGameManager _gameManager;

    [SerializeField] private Tilemap buildingGhostTilemap;

    private Vector3Int? lastCellWithBuildingGhost;

    [Inject]
    private void InitializeDependencies()
    {
        _tileSelector.TilePointerEntered += OnTilePointerEntered;
        
        _playerController.PlayerModeSet += OnPlayerModeSet;
        
        _gameManager.GameStageChanged += OnGameStageChanged;
    }

    private void OnGameStageChanged(GameStage stage)
    {
        if (stage is GameStage.Ended or GameStage.Pause)
        {
            ClearGhost();
        }
    }

    private void OnPlayerModeSet(PlayerMode mode)
    {
        if (mode != PlayerMode.Building)
        {
            ClearGhost();
        }
    }

    private void OnTilePointerEntered(Vector3Int cell, TileData tile)
    {
        MoveBuildingGhost(cell, tile);
    }

    private void ClearGhost()
    {
        if (lastCellWithBuildingGhost is not null)
        {
            buildingGhostTilemap.SetTile((Vector3Int)lastCellWithBuildingGhost, null);
            lastCellWithBuildingGhost = null;
        }
    }
    
    private void MoveBuildingGhost(Vector3Int cell, TileData tile)
    {
        // If no building is selected, return immediately
        if (_buildingController.SelectedBuilding is null)
        {
            return;
        }
        
        // Clear the last cell's ghost if it exists
        if (lastCellWithBuildingGhost.HasValue)
        {
            buildingGhostTilemap.SetTile(lastCellWithBuildingGhost.Value, null);
        }

        var tileData = _tileMapData.GetData((Vector2Int)cell);
        
        // Set the ghost on the new cell if building is possible
        if (_buildingController.CanBuildOnTile(tileData))
            buildingGhostTilemap.SetTile(cell, _buildingController.SelectedBuilding.Tile);

        lastCellWithBuildingGhost = cell;
    }
}