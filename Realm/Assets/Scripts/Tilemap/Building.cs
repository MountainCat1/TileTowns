using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(Building))]
public class Building : TileBase
{
    [field: SerializeField] public Tile Tile { get; set; }
    [field: SerializeField] public float Price { get; set; }

    public virtual void UpdateState(Vector3Int position, GameStateMutation mutation)
    {
    }

    public virtual string GetDescription()
    {
        return "NO DESCRIPTION??? WTH?!!!";
    }
}