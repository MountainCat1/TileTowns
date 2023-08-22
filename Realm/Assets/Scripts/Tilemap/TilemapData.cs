using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapData : MonoBehaviour
{
    public Dictionary<Vector3Int, CellData> Data { get; set; } = new();
    
    public void SetData(Vector3Int position, CellData data)
    {
        Data[position] = data;
    }

    public CellData GetData(Vector3Int position)
    {
        Data.TryGetValue(position, out var data);
        return data;
    }
}