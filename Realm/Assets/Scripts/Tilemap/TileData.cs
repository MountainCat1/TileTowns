using System;
using JetBrains.Annotations;
using UnityEngine;

public class TileData : IMutator
{
    // Events

    public event Action MutationChanged;
    
    //
    
    
    [CanBeNull] 
    public Building Building { get; private set; }
    public Vector3Int Position { get; private set; }

    public TileData(Vector3Int position)
    {
        Position = position;
    }

    public IGameStateTurnMutation GetMutation()
    {
        var stateChange = new GameStateTurnMutation(this);
        
        if (Building != null) 
            Building.CreateMutation(Position, stateChange);

        return stateChange;
    }

    public void SetBuilding(Building building)
    {
        Building = building;
        
        MutationChanged?.Invoke();
    }
}