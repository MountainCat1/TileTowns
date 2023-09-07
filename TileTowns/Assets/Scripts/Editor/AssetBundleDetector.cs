using System.IO;
using UnityEditor;

namespace Editor
{
    public class AssetBundleDetector : AssetPostprocessor
    {
        private const string AssetDirectory = "AssetResources";
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                // Check if the asset is in the "AssetBundles" directory
                if (!assetPath.StartsWith($"Assets/{AssetDirectory}"))
                {
                    // Enabling this will make so that all scripts outside of <AssetDirectory> will be removed from 
                    // asset bundles
                    // AssetImporter.GetAtPath(assetPath).assetBundleName = "";
                    continue;
                }

                // Get the folder name for the asset
                string folderName = Path.GetDirectoryName(assetPath)?.Replace($"Assets\\{AssetDirectory}\\", "");

                // Set the asset's assetBundleName to the folder name
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                importer.assetBundleName = folderName;
            }
        }
    }
}