using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoadManager : MonoBehaviour
{
    [Inject] protected ITileMapData TileMapData { get; private set; }
    private Dictionary<Vector2Int, bool> _roadMap = new();

    private void Start()
    {
        _roadMap = new Dictionary<Vector2Int, bool>();
        
        foreach (var tile in TileMapData.Data)
        {
            _roadMap.Add(tile.Value.Position, false);
        }
    }
    
    public void PlaceRoad(Vector2Int position)
    {
        _roadMap[position] = true;
        
        var adjacentTiles = GetAdjacentPositions(position);
        foreach (var adjacentPosition in adjacentTiles)
        {
            if (_roadMap.ContainsKey(adjacentPosition) && _roadMap[adjacentPosition])
            {
                UpdateRoad(adjacentPosition);
            }
        }
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
            // All sides have roads
        }
        else if (hasRoadAbove && hasRoadBelow && hasRoadLeft)
        {
            // Roads are from above, below, and left
        }
        else if (hasRoadAbove && hasRoadBelow && hasRoadRight)
        {
            // Roads are from above, below, and right
        }
        else if (hasRoadAbove && hasRoadLeft && hasRoadRight)
        {
            // Roads are from above, left, and right
        }
        else if (hasRoadBelow && hasRoadLeft && hasRoadRight)
        {
            // Roads are from below, left, and right
        }
        else if (hasRoadAbove && hasRoadBelow)
        {
            // Roads are from above and below
        }
        else if (hasRoadAbove && hasRoadLeft)
        {
            // Roads are from above and left
        }
        else if (hasRoadAbove && hasRoadRight)
        {
            // Roads are from above and right
        }
        else if (hasRoadBelow && hasRoadLeft)
        {
            // Roads are from below and left
        }
        else if (hasRoadBelow && hasRoadRight)
        {
            // Roads are from below and right
        }
        else if (hasRoadLeft && hasRoadRight)
        {
            // Roads are from left and right
        }
        else if (hasRoadAbove)
        {
            // There is a road from above
        }
        else if (hasRoadBelow)
        {
            // There is a road from below
        }
        else if (hasRoadLeft)
        {
            // There is a road from the left
        }
        else if (hasRoadRight)
        {
            // There is a road from the right
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
}