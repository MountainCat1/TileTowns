using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Buildings;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[InitializeOnLoad]
public class AssemblyReloadHandler
{
    static AssemblyReloadHandler()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private static void OnEditorUpdate()
    {
        // Unsubscribe from the update event to avoid running the logic every frame
        EditorApplication.update -= OnEditorUpdate;

        //! TODO
    }
}

public class CustomTileAssetGenerator : MonoBehaviour
{
    [MenuItem("Tools/Create All Tiles")]
    public static void CreateAllTileAssets()
    {
        CreateBuildingTiles();
    }

    public static void CreateBuildingTiles()
    {
        CreateCustomTiles<Building>("Buildings");
    }

    /// <summary>
    /// This method instantiates an object per class that derives from <typeparamref name="T"/>
    /// </summary>
    /// <param name="assetPath">Where create assets should be stored</param>
    /// <typeparam name="T">Partent Class</typeparam>
    public static void CreateCustomTiles<T>(string assetPath) where T : ScriptableObject
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type tileBaseType = typeof(TileBase);

        var types = assemblies.SelectMany(x => x.GetTypes());
        foreach (var type in types)
        {
            if (type == typeof(Woodmill))
                Debug.Log("XD");


            if (type != tileBaseType
                && !type.IsAbstract
                && !type.ContainsGenericParameters
                && type != typeof(Tile)
                && !type.IsSubclassOf(typeof(RuleTile))
                && type.IsSubclassOf(typeof(T)))
            {
                string fullAssetPath = Path.Combine("Assets", "Data", assetPath, type.Name + ".asset");

                // Check if an asset already exists at the specified path
                if (AssetDatabase.LoadAssetAtPath<ScriptableObject>(fullAssetPath))
                {
                    continue;
                }

                Debug.Log($"Creating tile asset {fullAssetPath}");

                var customTile = ScriptableObject.CreateInstance(type);

                AssetDatabase.CreateAsset(customTile, fullAssetPath);
            }
        }

        AssetDatabase.Refresh();
    }
}