using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(BuildingData))]
    public class BuildingData : ScriptableObject
    {
        [field: SerializeField] public Building BuildingPrefab { get; set; }

        [field: SerializeField] public float Price { get; set; }
    }
}