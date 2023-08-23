using UnityEngine;
using UnityEngine.Tilemaps;


public class BuildingController : MonoBehaviour
{
    private const int BuildingZIndex = 1;
    
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TileSelector tileSelector;
    [SerializeField] private TilemapData tilemapData;
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

    private void TileSelectorOnTilePointerEntered(Vector3Int cellPosition, CellData cellData)
    {
    }

    public void SelectBuilding(Building buildingPrefab)
    {
        _buildingToBuild = buildingPrefab;
    } 

    private void TileSelectorOnTilePointerClicked(Vector3Int cellPosition, CellData cellData)
    {
        BuildBuilding(cellData, _buildingToBuild);
    }
    


    // ReSharper disable once SuggestBaseTypeForParameter
    private void BuildBuilding(CellData cellData, Building building)
    {
        if(!CanBuildOnTile(cellData))
            return;

        var buildingCellPosition = new Vector3Int(cellData.Position.x, cellData.Position.y, BuildingZIndex);
        
        _tilemap.SetTile(buildingCellPosition , building);
        _tilemap.RefreshAllTiles();

        cellData.Building = building;
    }
    
    // private void BuildBuildingAsAnOject(CellData cellData, Building building)
    // {
    //     if(!CanBuildOnTile(cellData))
    //         return;
    //     
    //     var worlPosition = grid.GetCellCenterWorld(cellData.Position);
    //
    //     
    //     
    //     var building = Instantiate(building.gameObject, worlPosition, Quaternion.identity)
    //         .GetComponent<Building>();
    //     
    //     cellData.Building = building;
    //     
    //     tilemapData.SetData(cellData.Position, cellData);
    // }


    private bool CanBuildOnTile(CellData cellData)
    {
        if (cellData.Building is not null)
            return false;

        if (cellData.CellFeature is not null)
            return false;

        return true;
    }
}