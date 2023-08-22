using UnityEngine;

public class CellData
{
    public CellData(Vector3Int position)
    {
        Position = position;
    }

    public CellEntity Building { get; set; }
    public CellEntity CellFeature { get; set; }

    public Vector3Int Position { get; private set; }
    
}