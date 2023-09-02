using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface ITileMapData
{
    IReadOnlyDictionary<Vector3Int, TileData> Data { get; }
    TileData GetData(Vector3Int cell);
}

public class TileMapData : MonoBehaviour, ITileMapData
{
    [Inject] private IGameManager _gameManager; 
    
    public IReadOnlyDictionary<Vector3Int, TileData> Data => _data;
    private readonly Dictionary<Vector3Int, TileData> _data  = new();

    private Tilemap _tilemap;

    private void OnEnable()
    {
        _gameManager.LevelLoaded += GameManagerOnGameLoaded;
    }

    private void GameManagerOnGameLoaded()
    {
        InstantiateData(_gameManager.Tilemap);
    }

    public TileData GetData(Vector3Int position)
    {
        _data.TryGetValue(position, out var data);

        if (data is null)
            throw new IndexOutOfRangeException();

        return data;
    }
    
    public void InstantiateData(Tilemap tilemap)
    {
        if (_data.Any())
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
                        _data.Add(cellPosition, cellData);
                    }
                }
            }
        }
    }
    
    private TileData GetInitialCellData(Vector3Int cellPosition, TileBase tileBase)
    {
        return new TileData(cellPosition);
    }
}