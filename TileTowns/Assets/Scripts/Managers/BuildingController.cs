using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface IBuildingController
{
    #region MyRegion
    event Action PlaceBuildingFailed;
    event Action<Building, TileData> PlacedBuilding;
    event Action<Building> BuildingSelected;
    #endregion

    void SelectBuilding(Building building);
    void BuildBuilding(TileData tileData, Building building);
    bool CanBuildOnTile(TileData tileData);
    
    [CanBeNull] Building SelectedBuilding { get; }
}

public class BuildingController : MonoBehaviour, IBuildingController
{
    private const int BuildingZIndex = 1;

    #region Events

    public event Action PlaceBuildingFailed;
    public event Action<Building, TileData> PlacedBuilding;
    public event Action<Building> BuildingSelected;

    #endregion

    [Inject] private DiContainer _container;
    [Inject] private IGameManager _gameManager;
    [Inject] private ITileSelector _tileSelector;
    [Inject] private IResourceManager _resourceManager;

    [SerializeField] private Grid grid;

    private Tilemap _tilemap;
    public Building SelectedBuilding { get; private set; }


    private void OnEnable()
    {
        _tileSelector.TilePointerClicked += TileSelectorOnTilePointerClicked;
        _tileSelector.TilePointerEntered += TileSelectorOnTilePointerEntered;

        _gameManager.LevelLoaded += GameManagerOnGameLoaded;
    }


    private void OnDisable()
    {
        _tileSelector.TilePointerClicked -= TileSelectorOnTilePointerClicked;
        _tileSelector.TilePointerEntered -= TileSelectorOnTilePointerEntered;

        _gameManager.LevelLoaded -= GameManagerOnGameLoaded;
    }

    private void GameManagerOnGameLoaded()
    {
        _tilemap = _gameManager.Tilemap;

        var buildings = _gameManager.LevelConfig.BuildingSet;

        foreach (var building in buildings)
        {
            _container.Inject(building);
        }
    }

    private void TileSelectorOnTilePointerEntered(Vector3Int cellPosition, TileData tileData)
    {
    }

    public void SelectBuilding(Building building)
    {
        SelectedBuilding = building;
        BuildingSelected?.Invoke(building);
    }

    private void TileSelectorOnTilePointerClicked(Vector3Int cellPosition, TileData tileData)
    {
        if (SelectedBuilding is not null)
            BuildBuilding(tileData, SelectedBuilding);
    }

    public void BuildBuilding(TileData tileData, Building building)
    {
        if (!CanBuildOnTile(tileData))
        {
            PlaceBuildingFailed?.Invoke();
            return;
        }

        if (!_resourceManager.SpendMoney(building.Price))
        {
            PlaceBuildingFailed?.Invoke();
            return;
        }

        var buildingCellPosition = new Vector3Int(tileData.Position.x, tileData.Position.y, BuildingZIndex);

        _tilemap.SetTile(buildingCellPosition, building.Tile);
        _tilemap.RefreshAllTiles();

        tileData.SetBuilding(building);
        
        PlacedBuilding?.Invoke(building, tileData);
    }
    
    public bool CanBuildOnTile(TileData tileData)
    {
        if (tileData is null)
            return false;
        
        if (tileData.Building is not null)
            return false;

        return true;
    }
}