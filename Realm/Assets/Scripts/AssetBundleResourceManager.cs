using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class AssetBundleResourceManager : IResourceManager
{
    public const string AssetBundlesSubDirectory = "AssetBundles/";
    private readonly string assetDirectory;

    public AssetBundleResourceManager()
    {
#if UNITY_EDITOR
        assetDirectory = Application.dataPath;
#else
        assetDirectory = Application.streamingAssetsPath;
#endif
    }


    public IEnumerable<T> LoadAllAssets<T>(string bundleName) where T : Object
    {
        var path = new DirectoryInfo(Path.Combine(assetDirectory, $"{AssetBundlesSubDirectory}{bundleName}")).FullName;
        var assetBundle = AssetBundle.LoadFromFile(path);
        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return default;
        }

        T[] assets = new T[assetBundle.GetAllAssetNames().Length];
        for (int i = 0; i < assetBundle.GetAllAssetNames().Length; i++)
        {
            assets[i] = assetBundle.LoadAsset<T>(assetBundle.GetAllAssetNames()[i]);
        }

        assetBundle.Unload(false);

        return assets;
    }

    public T LoadAsset<T>(string bundleName, string assetName) where T : Object
    {
        var assetBundle = AssetBundle.LoadFromFile(Path.Combine(assetDirectory, $"{AssetBundlesSubDirectory}{bundleName}"));
        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return default;
        }

        var loadedAsset = assetBundle.LoadAsset<T>(assetName);

        assetBundle.Unload(false);

        return loadedAsset;
    }

    public T[] LoadSubAssets<T>(string bundleName, string assetName) where T : Object
    {
        var fullBundleName =
            new DirectoryInfo(Path.Combine(assetDirectory, $"{AssetBundlesSubDirectory}{bundleName}"))
                .FullName;

        AssetBundle assetBundle = AssetBundle.GetAllLoadedAssetBundles().FirstOrDefault(x => x.name == bundleName);

        if (assetBundle == null)
        {
            assetBundle = AssetBundle.LoadFromFile(fullBundleName);
            if (assetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return default;
            }
        }


        var loadedAsset = assetBundle.LoadAssetWithSubAssets<T>(assetName);

        assetBundle.Unload(false);

        return loadedAsset;
    }

    public string[] GetAssetsNamesInBundle(string bundleName)
    {
        var assetBundle = AssetBundle.LoadFromFile(Path.Combine(assetDirectory,
            $"{AssetBundlesSubDirectory}{bundleName}"));
        if (assetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return default;
        }

        var names = assetBundle.GetAllAssetNames();

        assetBundle.Unload(false);

        return names;
    }
}