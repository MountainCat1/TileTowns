using UnityEngine;


public class BuildingController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private TileSelector tileSelector;
    [SerializeField] private TilemapData tilemapData;
    [SerializeField] private Grid grid;

    [SerializeField] private Building _buildingToBuild;
    
    private void Awake()
    {
    }

    
    private void OnEnable()
    {
        tileSelector.TilePointerClicked += TileSelectorOnTilePointerClicked;
        tileSelector.TilePointerEntered += TileSelectorOnTilePointerEntered;
    }
    
    private void OnDisable()
    {
        tileSelector.TilePointerClicked -= TileSelectorOnTilePointerClicked;
        tileSelector.TilePointerEntered -= TileSelectorOnTilePointerEntered;
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
    private void BuildBuilding(CellData cellData, Building buildingPrefab)
    {
        if(!CanBuildOnTile(cellData))
            return;
        
        var worlPosition = grid.GetCellCenterWorld(cellData.Position);

        var building = Instantiate(buildingPrefab.gameObject, worlPosition, Quaternion.identity)
            .GetComponent<Building>();
        
        cellData.Building = building;
        
        tilemapData.SetData(cellData.Position, cellData);
    }

    private bool CanBuildOnTile(CellData cellData)
    {
        if (cellData.Building is not null)
            return false;

        if (cellData.CellFeature is not null)
            return false;

        return true;
    }
}