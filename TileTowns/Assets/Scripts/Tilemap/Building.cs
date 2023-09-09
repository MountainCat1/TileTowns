using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(Building))]
public class Building : ScriptableObject
{
    [field: SerializeField] public Tile Tile { get; set; }
    [field: SerializeField] public float Price { get; set; }

    public virtual void CreateMutation(Vector3Int position, IGameStateTurnMutation mutation)
    {
    }

    public virtual string GetDescription()
    {
        return "NO DESCRIPTION??? WTH?!!!";
    }
}