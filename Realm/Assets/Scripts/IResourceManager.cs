using System.Collections.Generic;
using UnityEngine;

public interface IResourceManager
{
    IEnumerable<T> LoadAllAssets<T>(string bundleName) where T : Object;
    T LoadAsset<T>(string bundleName, string assetName) where T : Object;
    T[] LoadSubAssets<T>(string bundleName, string assetName) where T : Object;
    string[] GetAssetsNamesInBundle(string bundleName);
}