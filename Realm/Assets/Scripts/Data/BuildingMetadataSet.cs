using System.IO;
using System.Linq;
using Data;
using UnityEditor;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Building Set", fileName = nameof(BuildingMetadataSet))]
    public class BuildingMetadataSet : ScriptableObject
    {
        [field: SerializeField] public BuildingMetadata[] Buildings { get; set; }
    }
}

