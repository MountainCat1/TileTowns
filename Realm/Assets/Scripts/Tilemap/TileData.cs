using Data;
using UnityEngine;

public class TileData
{
    public TileData(Vector3Int position)
    {
        Position = position;
    }

    public BuildingBehaviour BuildingBehaviour { get; set; }

    public Vector3Int Position { get; private set; }

    public void OnTurn(Vector3Int position)
    {
        if (BuildingBehaviour)
            BuildingBehaviour.OnTurn(position);
    }
}