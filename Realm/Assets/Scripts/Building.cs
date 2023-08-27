using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : TileEntity
{
    [field: SerializeField] public TileBase TileBase { get; set; }
}