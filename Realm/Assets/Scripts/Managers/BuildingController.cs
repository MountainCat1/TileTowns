using Data;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;


public interface IBuildingController
{
    void SelectBuilding(BuildingData buildingData);
    void BuildBuilding(TileData tileData, BuildingData building);
}

public class BuildingController : MonoBehaviour, IBuildingController
{
    private const int BuildingZIndex = 1;

    [Inject] private ILevelManager _levelManager;
    [Inject] private ITileSelector _tileSelector;

    [SerializeField] private Grid grid;

    private BuildingData _selectedBuilding;
    private Tilemap _tilemap;


    private void OnEnable()
    {
        _tileSelector.TilePointerClicked += TileSelectorOnTilePointerClicked;
        _tileSelector.TilePointerEntered += TileSelectorOnTilePointerEntered;

        _levelManager.LevelLoaded += LevelManagerOnLevelLoaded;
    }


    private void OnDisable()
    {
        _tileSelector.TilePointerClicked -= TileSelectorOnTilePointerClicked;
        _tileSelector.TilePointerEntered -= TileSelectorOnTilePointerEntered;

        _levelManager.LevelLoaded -= LevelManagerOnLevelLoaded;
    }

    private void LevelManagerOnLevelLoaded()
    {
        _tilemap = _levelManager.Tilemap;
    }

    private void TileSelectorOnTilePointerEntered(Vector3Int cellPosition, TileData tileData)
    {
    }

    public void SelectBuilding(BuildingData buildingData)
    {
        _selectedBuilding = buildingData;
    }

    private void TileSelectorOnTilePointerClicked(Vector3Int cellPosition, TileData tileData)
    {
        if (_selectedBuilding is not null)
            BuildBuilding(tileData, _selectedBuilding);
    }


    // ReSharper disable once SuggestBaseTypeForParameter
    public void BuildBuilding(TileData tileData, BuildingData building)
    {
        if (!CanBuildOnTile(tileData))
            return;

        var buildingCellPosition = new Vector3Int(tileData.Position.x, tileData.Position.y, BuildingZIndex);

        _tilemap.SetTile(buildingCellPosition, building.BuildingPrefab);
        _tilemap.RefreshAllTiles();

        tileData.Building = building.BuildingPrefab;
    }

    // private void BuildBuildingAsAnOject(TileData tileData, Building building)
    // {
    //     if(!CanBuildOnTile(tileData))
    //         return;
    //     
    //     var worlPosition = grid.GetCellCenterWorld(tileData.Position);
    //
    //     
    //     
    //     var building = Instantiate(building.gameObject, worlPosition, Quaternion.identity)
    //         .GetComponent<Building>();
    //     
    //     tileData.Building = building;
    //     
    //     tileMapData.SetData(tileData.Position, tileData);
    // }


    private bool CanBuildOnTile(TileData tileData)
    {
        if (tileData.Building is not null)
            return false;

        if (tileData.TileFeature is not null)
            return false;

        return true;
    }
}