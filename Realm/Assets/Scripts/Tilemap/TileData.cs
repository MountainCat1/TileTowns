using Data;
using JetBrains.Annotations;
using UnityEngine;

public class TileData
{
    public TileData(Vector3Int position)
    {
        Position = position;
    }

    [CanBeNull] public Building BuildingBehaviour { get; set; }

    public Vector3Int Position { get; private set; }

    public void OnTurn(Vector3Int position)
    {
        if (BuildingBehaviour != null) 
            BuildingBehaviour.OnTurn(position);
    }
}