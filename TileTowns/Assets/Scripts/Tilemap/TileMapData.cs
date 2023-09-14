using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface ITileMapData
{
    event Action<TileData> TileAdded;

    IReadOnlyDictionary<Vector2Int, TileData> Data { get; }
    IEnumerable<TileData> TileData { get; }
    TileData GetData(Vector2Int tilePosition);
    public int AssignedWorkers { get; }
}

public class TileMapData : MonoBehaviour, ITileMapData
{
    // Events

    public event Action<TileData> TileAdded;

    // 

    [Inject] private IGameManager _gameManager;
    [Inject] private IGameState _gameState;
    [Inject] private ITileFeatureMap _tileFeatureMap;

    public IReadOnlyDictionary<Vector2Int, TileData> Data => _data;
    public IEnumerable<TileData> TileData => Data.Values;

    public int AssignedWorkers { get; private set; }

    private readonly Dictionary<Vector2Int, TileData> _data = new();
    private Tilemap _tilemap;

    private void OnEnable()
    {
        _gameManager.LevelLoaded += GameManagerOnGameLoaded;
        _gameState.MutationChanged += OnMutationChanged;
    }

    private void OnMutationChanged()
    {
        AssignedWorkers = TileData.Sum(x => x.WorkersAssigned);
    }

    private void GameManagerOnGameLoaded()
    {
        InstantiateData(_gameManager.Tilemap);
    }

    public TileData GetData(Vector2Int position)
    {
        _data.TryGetValue(position, out var data);

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
                    TileBase tile = _tilemap.GetTile(cellPosition);

                    if (tile is null)
                        continue;

                    // Get the tile one cell above
                    Vector3Int aboveCellPosition = cellPosition + new Vector3Int(0, 0, 1);
                    TileBase aboveTile = _tilemap.GetTile(aboveCellPosition);
                        
                    TileFeature tileFeature = _tileFeatureMap.GetMapping(aboveTile);
                    
                    AddTileData((Vector2Int)cellPosition, tile, tileFeature);
                }
            }
        }
    }

    private TileData AddTileData(Vector2Int cellPosition, TileBase tileBase, TileFeature feature)
    {
        var tileData = new TileData(cellPosition, feature, tileBase);

        _data.Add(cellPosition, tileData);

        TileAdded?.Invoke(tileData);

        return tileData;
    }
}