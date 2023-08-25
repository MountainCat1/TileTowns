using UnityEngine;

public class SliceData
{
    public SliceData(Vector3Int position)
    {
        Position = position;
    }

    public CellEntity Building { get; set; }
    public CellEntity CellFeature { get; set; }

    public Vector3Int Position { get; private set; }
    
}