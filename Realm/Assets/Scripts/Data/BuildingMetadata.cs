using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(BuildingMetadata))]
    public class BuildingMetadata : ScriptableObject
    {
        [field: SerializeField] public Building BuildingPrefab { get; set; }

        [field: SerializeField] public float Price { get; set; }
    }
}