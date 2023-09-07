using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Data.Editor
{
    [CustomEditor(typeof(BuildingMetadataSet))]
    public class BuildingMetadataSetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BuildingMetadataSet buildingSet = (BuildingMetadataSet)target;

            if (GUILayout.Button("Add Buildings From Directory"))
            {
                string directoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(buildingSet));

                string[] buildingScriptGuids = AssetDatabase.FindAssets($"t:{nameof(Building)}", new string[] { directoryPath });

                buildingSet.Buildings = buildingScriptGuids
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .Where(path => Path.GetFileNameWithoutExtension(path) != "BuildingMetadata")
                    .Select(path => AssetDatabase.LoadAssetAtPath<Building>(path))
                    .Where(x => x is not null)
                    .ToArray();
            }
        }
    }
}