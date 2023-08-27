using UnityEngine;

public class TileData
{
    public TileData(Vector3Int position)
    {
        Position = position;
    }

    public TileEntity Building { get; set; }
    public TileEntity TileFeature { get; set; }

    public Vector3Int Position { get; private set; }
    
}