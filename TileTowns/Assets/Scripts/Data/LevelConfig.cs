using Data;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Level Config")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public LevelDescriptor LevelDescriptor { get; set; }
    [field: SerializeField] public BuildingMetadataSet BuildingSet { get; set; }
}



