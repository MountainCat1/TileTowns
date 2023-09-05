using System.Collections.Generic;
using UnityEngine;

public static class GameResources
{
    private static IAssetManager AssetManager { get; }

    static GameResources()
    {
 #if UNITY_EDITOR
        // TODO: start using EditorAssetsResourceManager 
        AssetManager = new EditorAssetManager();
#else
        AssetManager = new AssetBundleAssetManager();
#endif
    }

    public static IEnumerable<T> LoadAll<T>(string bundleName) where T : Object
    {
        return AssetManager.LoadAllAssets<T>(bundleName);
    }

    public static T Load<T>(string bundleName, string assetName) where T : Object
    {
        return AssetManager.LoadAsset<T>(bundleName, assetName);
    }

    public static T[] LoadSubAssets<T>(string bundleName, string assetName) where T : Object
    {
        return AssetManager.LoadSubAssets<T>(bundleName, assetName);
    }

    public static string[] GetAssetNames(string bundleName)
    {
        return AssetManager.GetAssetsNamesInBundle(bundleName);
    }
}