using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapData : MonoBehaviour
{

    [SerializeField] private LevelManager levelManager; 
    
    public Dictionary<Vector3Int, CellData> Data { get; set; } = new();

    private Tilemap _tilemap;

    private void OnEnable()
    {
        levelManager.LevelLoaded += LevelManagerOnLevelLoaded;
    }

    private void LevelManagerOnLevelLoaded()
    {
        InstantiateData(levelManager.Tilemap);
    }

    public void SetData(Vector3Int position, CellData data)
    {
        if(!Data.TryGetValue(position, out _))
            throw new IndexOutOfRangeException();
            
        Data[position] = data;
    }

    public CellData GetData(Vector3Int position)
    {
        Data.TryGetValue(position, out var data);

        if (data is null)
            throw new IndexOutOfRangeException();

        return data;
    }
    
    public void InstantiateData(Tilemap tilemap)
    {
        if (Data.Any())
            throw new InvalidOperationException();

        _tilemap = tilemap;
        
        if (_tilemap != null)
        {
            BoundsInt bounds = _tilemap.cellBounds;
            TileBase[] allTiles = _tilemap.GetTilesBlock(bounds);

            for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
            {
                for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    TileBase tile = allTiles[(x - bounds.x) + (y - bounds.y) * bounds.size.x];

                    if (tile != null)
                    {
                        var cellData = GetInitialCellData(cellPosition, tile);
                        Data.Add(cellPosition, cellData);
                    }
                }
            }
        }
    }
    
    private CellData GetInitialCellData(Vector3Int cellPosition, TileBase tileBase)
    {
        return new CellData(cellPosition);
    }
}