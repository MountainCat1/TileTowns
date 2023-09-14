using System;
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

    protected TileData[] GetAdjacentTiles(Vector2Int position)
    {
        TileData[] neighbours = new[]
        {
            TileMapData.Data[position + new Vector2Int(0, 1)],
            TileMapData.Data[position + new Vector2Int(0, -1)],
            TileMapData.Data[position + new Vector2Int(1, 0)],
            TileMapData.Data[position + new Vector2Int(-1, 0)],
        };
        
        return neighbours;
    }
}