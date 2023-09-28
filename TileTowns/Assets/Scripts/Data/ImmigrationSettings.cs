using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class ImmigrationSettings
    {
        [field: SerializeField] public float ImmigrationPerPopulation { get; set; } = 100;
        [field: SerializeField] public float ImmigrationPerFreeHousing { get; set; } = 0.2f;
        [field: SerializeField] public float ImmigrationPerFreeJob { get; set; } = 0.2f;
    }
}