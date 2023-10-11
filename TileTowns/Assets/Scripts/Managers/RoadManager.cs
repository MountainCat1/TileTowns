using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface IRoadManager
{
    event Action<ITileData> RoadPlaced;
    void CreateRoad(ITileData tileData);
}

public class RoadManager : MonoBehaviour, IRoadManager
{
    private const int RoadZIndex = 1;

    #region Events

    public event Action<ITileData> RoadPlaced;

    #endregion
    
    [Inject] private IGameManager _gameManager;
    [Inject] private IBuildingController _buildingController;
    [Inject] protected ITileMapData TileMapData { get; private set; }

    [SerializeField] public Tile upDownLeftRightRoad;
    [SerializeField] public Tile upLeftRightRoad;
    [SerializeField] public Tile downLeftRightRoad;
    [SerializeField] public Tile upDownLeftRoad;
    [SerializeField] public Tile upDownRightRoad;
    [SerializeField] public Tile upRightRoad;
    [SerializeField] public Tile downLeftRoad;
    [SerializeField] public Tile upDownRoad;
    [SerializeField] public Tile downRightRoad;
    [SerializeField] public Tile upLeftRoad;
    [SerializeField] public Tile leftRightRoad;
    [SerializeField] public Tile upRoad;
    [SerializeField] public Tile rightRoad;
    [SerializeField] public Tile downRoad;
    [SerializeField] public Tile leftRoad;
    
    private Tilemap _tilemap;
    private Dictionary<Vector2Int, bool> _roadMap = new();
    
    [Inject]
    private void Construct()
    {
        _gameManager.LevelLoaded += InitializeRoadMap;
        _gameManager.LevelLoaded += OnGameLoaded;
        
        _buildingController.BuildingPlaced += (_, tileData) => CreateRoad(tileData);
    }
    
    private void OnGameLoaded()
    {
        _tilemap = _gameManager.Tilemap;
    }

    private void InitializeRoadMap()
    {
        _roadMap = new Dictionary<Vector2Int, bool>();
        
        foreach (var tile in TileMapData.Data)
        {
            _roadMap.Add(tile.Value.Position, false);
        }
    }

    public void CreateRoad(ITileData tileData)
    {
        var position = tileData.Position;
        _roadMap[position] = true;
        
        var adjacentTiles = GetAdjacentPositions(position);
        foreach (var adjacentPosition in adjacentTiles)
        {
            if (_roadMap.ContainsKey(adjacentPosition) && _roadMap[adjacentPosition])
            {
                UpdateRoad(adjacentPosition);
            }
        }
        
        UpdateRoad(position);
        
        RoadPlaced?.Invoke(tileData);
    }
    private void UpdateRoad(Vector2Int position)
    {
        if (_roadMap == null)
        {
            // Handle the case where _roadMap is null
            return;
        }

        bool hasRoadAbove = false;
        bool hasRoadBelow = false;
        bool hasRoadLeft = false;
        bool hasRoadRight = false;

        Vector2Int abovePosition = position + new Vector2Int(0, 1);
        Vector2Int belowPosition = position + new Vector2Int(0, -1);
        Vector2Int leftPosition = position + new Vector2Int(-1, 0);
        Vector2Int rightPosition = position + new Vector2Int(1, 0);

        hasRoadAbove = HasRoad(abovePosition);
        hasRoadBelow = HasRoad(belowPosition);
        hasRoadLeft = HasRoad(leftPosition);
        hasRoadRight = HasRoad(rightPosition);

        int roadConfig = 0;

        if (hasRoadAbove) roadConfig |= 1;
        if (hasRoadBelow) roadConfig |= 2;
        if (hasRoadLeft) roadConfig |= 4;
        if (hasRoadRight) roadConfig |= 8;
        switch (roadConfig)
        {
            // case 0: SetRoad(position, noRoad); break;
            case 1: SetRoad(position, upRoad); break;
            case 2: SetRoad(position, downRoad); break;
            case 4: SetRoad(position, leftRoad); break;
            case 8: SetRoad(position, rightRoad); break;
            case 3: SetRoad(position, upDownRoad); break;
            case 5: SetRoad(position, upLeftRoad); break;
            case 6: SetRoad(position, downLeftRoad); break;
            case 9: SetRoad(position, upRightRoad); break;
            case 10: SetRoad(position, downRightRoad); break;
            case 12: SetRoad(position, leftRightRoad); break;
            case 7: SetRoad(position, upDownLeftRoad); break;
            case 11: SetRoad(position, upDownRightRoad); break;
            case 13: SetRoad(position, upLeftRightRoad); break;
            case 14: SetRoad(position, downLeftRightRoad); break;
            case 15: SetRoad(position, upDownLeftRightRoad); break;
            // default: SetRoad(position, noRoad); break;
        }
    }

    private bool HasRoad(Vector2Int position)
    {
        return _roadMap.ContainsKey(position) && _roadMap[position];
    }

    private IEnumerable<Vector2Int> GetAdjacentPositions(Vector2Int position)
    {
        Vector2Int[] neighbours =
        {
            position + new Vector2Int(0, 1),
            position + new Vector2Int(0, -1),
            position + new Vector2Int(1, 0),
            position + new Vector2Int(-1, 0),
        };
        
        return neighbours;
    }
    
    private void SetRoad(Vector2Int position, TileBase roadTile)
    {
        var position3d = new Vector3Int(position.x, position.y, RoadZIndex);
        
        _tilemap.SetTile(position3d, roadTile);
        _tilemap.RefreshAllTiles();
    }
}