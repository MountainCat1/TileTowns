using System;
using JetBrains.Annotations;
using UnityEngine;

public class TileData
{
    // Events

    public event Action Changed;
    
    //
    
    
    [CanBeNull] 
    public Building Building { get; private set; }
    public Vector3Int Position { get; private set; }

    public TileData(Vector3Int position)
    {
        Position = position;
    }

    public GameStateChange GetChange()
    {
        var stateChange = GameStateChange.New(this);
        
        if (Building != null) 
            Building.UpdateState(Position, stateChange);

        return stateChange;
    }

    public void SetBuilding(Building building)
    {
        Building = building;
        
        Changed?.Invoke();
    }
}