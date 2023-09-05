using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

public class EditorAssetManager : IAssetManager
{
    public const string AssetResourcesDirectory = "AssetResources/";

    public T LoadAsset<T>(string bundleName, string assetName) where T : Object
    {
        string folderPath = $"Assets/{AssetResourcesDirectory}{bundleName}";
        string[] assetGUIDs = AssetDatabase.FindAssets(assetName, new []{folderPath});
        foreach (string guid in assetGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);
            if (fileName == assetName)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(path);
                return asset;
            }
        }

        return null;
    }

    public IEnumerable<T> LoadAllAssets<T>(string bundleName) where T : Object
    {
        string path = Path.Combine("Assets/", AssetResourcesDirectory, bundleName);
        string[] files = AssetDatabase.FindAssets("", new[] { path });
        var loadedAssets = new List<T>();

        foreach (string file in files)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(file);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            loadedAssets.Add(asset);
        }

        return loadedAssets;
    }
    
    public T[] LoadSubAssets<T>(string bundleName, string assetName) where T : Object
    {
        throw new System.NotImplementedException();
    }

    public string[] GetAssetsNamesInBundle(string bundleName)
    {
        throw new System.NotImplementedException();
    }
}

#endif

