using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface ITileData
{
    Vector2Int Position { get; }
    int WorkersAssigned { get; set; }
    TileFeature Feature { get; set; }
}

public class TileData : IMutator, ITileData
{
    // Events

    public event Action MutationChanged;
    
    //
    
    [CanBeNull] 
    public Building Building { get; private set; }

    public TileFeature Feature { get; set; }
    public TileBase TileBase { get; set; }
    public Vector2Int Position { get; private set; }

    public int WorkersAssigned { get; set; }

    public TileData(Vector2Int position, TileFeature tileFeature, TileBase tileBase)
    {
        Position = position;
        Feature = tileFeature;
        TileBase = tileBase;
    }

    public IGameStateTurnMutation GetMutation()
    {
        var stateChange = new GameStateTurnMutation(this);
        
        if (Building != null) 
            Building.UpdateMutation(this, stateChange);

        return stateChange;
    }
    
    public IPersistentModifier GetPersistentModifier()
    {
        var persistentModifer = new PersistentModifier();
        
        if(Building != null)
            Building.UpdateModifier(this, persistentModifer);

        return persistentModifer;
    }

    public void SetBuilding(Building building)
    {
        Building = building;
        
        MutationChanged?.Invoke();
    }

    public bool AddWorker()
    {
        if (Building is null)
            return false;

        if (Building.WorkSlots - WorkersAssigned <= 0)
            return false;

        WorkersAssigned++;
        
        MutationChanged?.Invoke();
        return true;
    }

    public bool RemoveWorker()
    {
        if (Building is null)
            return false;

        if (WorkersAssigned == 0)
            return false;

        WorkersAssigned--;
        
        MutationChanged?.Invoke();
        return true;
    }
}