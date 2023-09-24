using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public class RoadManager : MonoBehaviour
{
    private event Action<Building, ITileData> RoadPlaced;

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
    private const int RoadZIndex = 1;
    
    private void Start()
    {
        _buildingController.PlacedBuilding += CreateRoad;
        _gameManager.LevelLoaded += InitializeRoadMap;
        _gameManager.LevelLoaded += OnGameLoaded;
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

    private void CreateRoad(Building building, ITileData tileData)
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
    }

    private void UpdateRoad(Vector2Int position)
    {
        bool hasRoadAbove = false;
        bool hasRoadBelow = false;
        bool hasRoadLeft = false;
        bool hasRoadRight = false;

        if (_roadMap[position + new Vector2Int(0, 1)])
        {
            hasRoadAbove = true;
        }

        if (_roadMap[position + new Vector2Int(0, -1)])
        {
            hasRoadBelow = true;
        }

        if (_roadMap[position + new Vector2Int(1, 0)])
        {
            hasRoadRight = true;
        }

        if (_roadMap[ position + new Vector2Int(-1, 0)])
        {
            hasRoadLeft = true;
        }
        
        if (hasRoadAbove && hasRoadBelow && hasRoadLeft && hasRoadRight)
        {
            SetRoad(position, upDownLeftRightRoad);
        }
        else if (hasRoadAbove && hasRoadBelow && hasRoadLeft)
        {
            SetRoad(position, upDownLeftRoad);
        }
        else if (hasRoadAbove && hasRoadBelow && hasRoadRight)
        {
            SetRoad(position, upDownRightRoad);
        }
        else if (hasRoadAbove && hasRoadLeft && hasRoadRight)
        {
            SetRoad(position, upLeftRightRoad);
        }
        else if (hasRoadBelow && hasRoadLeft && hasRoadRight)
        {
            SetRoad(position ,downLeftRightRoad);
        }
        else if (hasRoadAbove && hasRoadBelow)
        {
            SetRoad(position, upDownRoad);
        }
        else if (hasRoadAbove && hasRoadLeft)
        {
            SetRoad(position, upLeftRoad);
        }
        else if (hasRoadAbove && hasRoadRight)
        {
            SetRoad(position, upRightRoad);
        }
        else if (hasRoadBelow && hasRoadLeft)
        {
            SetRoad(position, downLeftRoad);
        }
        else if (hasRoadBelow && hasRoadRight)
        {
            SetRoad(position, downRightRoad);
        }
        else if (hasRoadLeft && hasRoadRight)
        {
            SetRoad(position, leftRightRoad);
        }
        else if (hasRoadAbove)
        {
            SetRoad(position, upRoad);
        }
        else if (hasRoadBelow)
        {
            SetRoad(position, downRoad);
        }
        else if (hasRoadLeft)
        {
            SetRoad(position, leftRoad);
        }
        else if (hasRoadRight)
        {
            SetRoad(position, rightRoad);
        }
        else
        {
            // No roads around
        }
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