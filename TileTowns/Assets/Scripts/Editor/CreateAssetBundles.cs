using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";

        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, 
            BuildAssetBundleOptions.None, 
            BuildTarget.StandaloneWindows);

        
        var targetAssetBundleDirectory 
            = Path.Combine(new DirectoryInfo(Application.dataPath).Parent!.ToString(), "Build/SpaceGame_Data/StreamingAssets/AssetBundles");

        Directory.CreateDirectory(targetAssetBundleDirectory);
        
        CopyAll(assetBundleDirectory, targetAssetBundleDirectory);
    }
    
    
    private static void CopyAll(string source, string target)
    {
        Directory.CreateDirectory(target);

        foreach (var file in Directory.GetFiles(source))
        {
            string targetFile = Path.Combine(target, Path.GetFileName(file));
            File.Copy(file, targetFile, true);
        }

        foreach (var directory in Directory.GetDirectories(source))
        {
            string targetDir = Path.Combine(target, Path.GetFileName(directory));
            CopyAll(directory, targetDir);
        }
    }
}
