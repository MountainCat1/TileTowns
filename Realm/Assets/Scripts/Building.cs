using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : CellEntity
{
    [field: SerializeField] public TileBase TileBase { get; set; }
}