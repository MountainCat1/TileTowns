using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface IBuildingController
{
    #region Events

    event Action PlaceBuildingFailed;
    event Action<Building, TileData> PlacedBuilding;
    event Action<Building> BuildingSelected;
    event Action BuildingDeselected;

    #endregion

    void SelectBuilding(Building building);
    void BuildBuilding(TileData tileData, Building building);
    bool CanBuildOnTile(TileData tileData);

    [CanBeNull] Building SelectedBuilding { get; }
}

public class BuildingController : MonoBehaviour, IBuildingController
{
    private const int BuildingZIndex = 2;

    #region Events

    public event Action PlaceBuildingFailed;
    public event Action<Building, TileData> PlacedBuilding;
    public event Action<Building> BuildingSelected;
    public event Action BuildingDeselected;

    #endregion

    [Inject] private DiContainer _container;
    [Inject] private IGameManager _gameManager;
    [Inject] private ITileSelector _tileSelector;
    [Inject] private ITileMapData _tileMapData;
    [Inject] private IResourceManager _resourceManager;
    [Inject] private IPlayerController _playerController;

    [SerializeField] private Grid grid;

    private Tilemap _tilemap;
    public Building SelectedBuilding { get; private set; }


    private void OnEnable()
    {
        _tileSelector.TilePointerClicked += TileSelectorOnTilePointerClicked;
        _tileSelector.TilePointerEntered += TileSelectorOnTilePointerEntered;

        _gameManager.LevelLoaded += GameManagerOnGameLoaded;

        _playerController.PlayerModeSet += OnPlayerModeSet;
    }

    private void OnPlayerModeSet(PlayerMode mode)
    {
        if (mode != PlayerMode.Building)
        {
            DeselectBuilding();
        }
    }


    private void DeselectBuilding()
    {
        SelectedBuilding = null;
        BuildingDeselected?.Invoke();
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

        foreach (var initialBuilding in _gameManager.LevelConfig.InitialBuildings)
        {
            var tile = _tileMapData.Data[initialBuilding.position];
            var building = initialBuilding.building;
            
            PlaceBuilding(tile, building);
        }
    }

    private void TileSelectorOnTilePointerEntered(Vector3Int cellPosition, TileData tileData)
    {
    }

    public void SelectBuilding(Building building)
    {
        SelectedBuilding = building;
        _playerController.SetPlayerMode(PlayerMode.Building);
        BuildingSelected?.Invoke(building);
    }

    private void TileSelectorOnTilePointerClicked(Vector3Int cellPosition, TileData tileData)
    {
        if (SelectedBuilding is not null)
            BuildBuilding(tileData, SelectedBuilding);
    }

    public void PlaceBuilding(TileData tileData, Building building)
    {
        var buildingCellPosition = new Vector3Int(tileData.Position.x, tileData.Position.y, BuildingZIndex);

        _tilemap.SetTile(buildingCellPosition, building.Tile);
        _tilemap.RefreshAllTiles();

        tileData.SetBuilding(building);

        PlacedBuilding?.Invoke(building, tileData);
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
        
        PlaceBuilding(tileData, building);
    }

    public bool CanBuildOnTile(TileData tileData)
    {
        if (tileData is null)
            return false;

        if (tileData.Building is not null)
            return false;

        if (tileData.Feature != TileFeature.None)
            return false;
            
        return true;
    }
}