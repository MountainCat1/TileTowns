using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(BuildingData))]
    public class BuildingData : ScriptableObject
    {
        [field: SerializeField] public BuildingBehaviour BuildingBehaviourPrefab { get; set; }
        [field: SerializeField] public Tile Tile { get; set; }
        [field: SerializeField] public float Price { get; set; }
    }
}