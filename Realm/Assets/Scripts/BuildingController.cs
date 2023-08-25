using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;


public class BuildingController : MonoBehaviour
{
    private const int BuildingZIndex = 1;
    
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TileSelector tileSelector;
    [FormerlySerializedAs("tilemapData")] [SerializeField] private SliceMapData sliceMapData;
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

    private void TileSelectorOnTilePointerEntered(Vector3Int cellPosition, SliceData sliceData)
    {
    }

    public void SelectBuilding(Building buildingPrefab)
    {
        _buildingToBuild = buildingPrefab;
    } 

    private void TileSelectorOnTilePointerClicked(Vector3Int cellPosition, SliceData sliceData)
    {
        BuildBuilding(sliceData, _buildingToBuild);
    }
    


    // ReSharper disable once SuggestBaseTypeForParameter
    private void BuildBuilding(SliceData sliceData, Building building)
    {
        if(!CanBuildOnTile(sliceData))
            return;

        var buildingCellPosition = new Vector3Int(sliceData.Position.x, sliceData.Position.y, BuildingZIndex);
        
        _tilemap.SetTile(buildingCellPosition , building);
        _tilemap.RefreshAllTiles();

        sliceData.Building = building;
    }
    
    // private void BuildBuildingAsAnOject(SliceData sliceData, Building building)
    // {
    //     if(!CanBuildOnTile(sliceData))
    //         return;
    //     
    //     var worlPosition = grid.GetCellCenterWorld(sliceData.Position);
    //
    //     
    //     
    //     var building = Instantiate(building.gameObject, worlPosition, Quaternion.identity)
    //         .GetComponent<Building>();
    //     
    //     sliceData.Building = building;
    //     
    //     sliceMapData.SetData(sliceData.Position, sliceData);
    // }


    private bool CanBuildOnTile(SliceData sliceData)
    {
        if (sliceData.Building is not null)
            return false;

        if (sliceData.CellFeature is not null)
            return false;

        return true;
    }
}