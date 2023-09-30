using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ImmigrationSettings
    {
        [field: SerializeField] public float InitialImmigration { get; set; } = 50f;
        
        [field: SerializeField] public float ImmigrationPerPopulation { get; set; } = 100f;
        [field: SerializeField] public float ImmigrationForPopulation { get; set; } = -0.1f;
        [field: SerializeField] public float ImmigrationForFreeHousing { get; set; } = 0.2f;
        [field: SerializeField] public float ImmigrationForFreeJob { get; set; } = 0.2f;
    }
}