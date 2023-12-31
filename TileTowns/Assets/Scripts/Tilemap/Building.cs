﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

[CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(Building))]
public abstract class Building : ScriptableObject
{
    [field: SerializeField] public Tile Tile { get; set; }
    [field: SerializeField] public float Price { get; set; }
    public virtual int WorkSlots => 0;
    public virtual int Housing => 0;
    public virtual int ImmigrationChange => 0;

    [Inject] protected ITileMapData TileMapData { get; private set; }

    public virtual void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
    {
    }

    public virtual void UpdateModifier(ITileData tileData, IPersistentModifier persistentModifier)
    {
        persistentModifier.Housing = Housing;
        persistentModifier.WorkSlots = WorkSlots;
    }

    public virtual string GetDescription()
    {
        return "NO DESCRIPTION??? WTH?!!!";
    }

    public virtual void OnPlaced(ITileData tileData)
    {
        foreach (var adjacentTile in GetAdjacentTiles(tileData.Position))
        {
            adjacentTile.UpdateMutation();
        }
    }

    protected TileData[] GetAdjacentTiles(Vector2Int position)
    {
        List<TileData> neighbours = new List<TileData>();
    
        Vector2Int[] offsets = new[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        foreach (var offset in offsets)
        {
            Vector2Int adjacentPosition = position + offset;

            if (TileMapData.Data.ContainsKey(adjacentPosition))
            {
                neighbours.Add(TileMapData.Data[adjacentPosition]);
            }
        }
    
        return neighbours.ToArray();
    }
}