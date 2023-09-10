using System;
using JetBrains.Annotations;
using UnityEngine;

public interface ITileData
{
    Vector3Int Position { get; }
    int WorkersAssigned { get; set; }
}

public class TileData : IMutator, ITileData
{
    // Events

    public event Action MutationChanged;
    
    //
    
    [CanBeNull] 
    public Building Building { get; private set; }
    public Vector3Int Position { get; private set; }

    public int WorkersAssigned { get; set; }

    public TileData(Vector3Int position)
    {
        Position = position;
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
        return true;
    }
}