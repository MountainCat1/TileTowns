using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;


public class BuildingController : MonoBehaviour
{
    private const int BuildingZIndex = 1;
    
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TileSelector tileSelector;
    [FormerlySerializedAs("sliceMapData")] [SerializeField] private TileMapData tileMapData;
    [SerializeField] private Grid grid;

    [SerializeField] private Building _buildingToBuild;

    private Tilemap _tilemap;
    
    private void Awake()
    {
    }

    
    private void OnEnable()
    {
        tileSelector.TilePointerClicked += TileSelectorOnTilePointerClicked;
        tileSelector.TilePointerEntered += TileSelectorOnTilePointerEntered;
        
        levelManager.LevelLoaded += LevelManagerOnLevelLoaded;
    }

    

    private void OnDisable()
    {
        tileSelector.TilePointerClicked -= TileSelectorOnTilePointerClicked;
        tileSelector.TilePointerEntered -= TileSelectorOnTilePointerEntered;
        
        levelManager.LevelLoaded -= LevelManagerOnLevelLoaded;
    }
    
    private void LevelManagerOnLevelLoaded()
    {
        _tilemap = levelManager.Tilemap;
    }

    private void TileSelectorOnTilePointerEntered(Vector3Int cellPosition, TileData tileData)
    {
    }

    public void SelectBuilding(Building buildingPrefab)
    {
        _buildingToBuild = buildingPrefab;
    } 

    private void TileSelectorOnTilePointerClicked(Vector3Int cellPosition, TileData tileData)
    {
        BuildBuilding(tileData, _buildingToBuild);
    }
    


    // ReSharper disable once SuggestBaseTypeForParameter
    private void BuildBuilding(TileData tileData, Building building)
    {
        if(!CanBuildOnTile(tileData))
            return;

        var buildingCellPosition = new Vector3Int(tileData.Position.x, tileData.Position.y, BuildingZIndex);
        
        _tilemap.SetTile(buildingCellPosition , building);
        _tilemap.RefreshAllTiles();

        tileData.Building = building;
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