using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(Building))]
public class Building : TileBase
{
    [field: SerializeField] public Tile Tile { get; set; }
    [field: SerializeField] public float Price { get; set; }

    public virtual void ApplyMutation(Vector3Int position, GameStateTurnMutation mutation)
    {
    }

    public virtual string GetDescription()
    {
        return "NO DESCRIPTION??? WTH?!!!";
    }
}