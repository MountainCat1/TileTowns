using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(Building))]
public abstract class Building : ScriptableObject
{
    [field: SerializeField] public Tile Tile { get; set; }
    [field: SerializeField] public float Price { get; set; }
    public virtual int WorkSlots => 0;
    public virtual int Housing => 0;

    public virtual void UpdateMutation(Vector3Int position, IGameStateTurnMutation mutation)
    {
    }

    public virtual void UpdateModifier(Vector3Int cellPosition, IPersistentModifier persistentModifier)
    {
        persistentModifier.Housing = Housing;
        persistentModifier.WorkSlots = WorkSlots;
    }

    public virtual string GetDescription()
    {
        return "NO DESCRIPTION??? WTH?!!!";
    }
    
}