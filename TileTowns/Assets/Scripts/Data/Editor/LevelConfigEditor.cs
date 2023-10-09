using UnityEditor;
using UnityEngine;

namespace Data.Editor
{
    [CustomEditor(typeof(LevelConfig))]
    public class LevelConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // This will draw the default inspector content

            LevelConfig myScript = (LevelConfig)target;
            if (GUILayout.Button("Load Assets From Directory"))
            {
                LoadAssets(myScript);
            }
        }

        private void LoadAssets(LevelConfig config)
        {
            string assetPath = AssetDatabase.GetAssetPath(config);
            string path = System.IO.Path.GetDirectoryName(assetPath)!.Replace("\\", "/") + "/";

            Sprite thumbnail = AssetDatabase.LoadAssetAtPath<Sprite>(path + "Thumbnail.png"); 
            if (thumbnail != null)
                config.Thumbnail = thumbnail;

            BuildingMetadataSet buildingSet = AssetDatabase.LoadAssetAtPath<BuildingMetadataSet>(path + "BuildingSet.asset");
            if (buildingSet != null)
                config.BuildingSet = buildingSet;

            WinCondition winCondition = AssetDatabase.LoadAssetAtPath<WinCondition>(path + "WinCondition.asset"); 
            if (winCondition != null)
                config.WinCondition = winCondition;

            EditorUtility.SetDirty(config);
        }
    }
}