using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    // Events

    public event Action LevelLoaded;
    
    //
    
    [field: SerializeField] public Level LevelConfig { get; set; }

    [SerializeField] private Grid tilemapContainer;

    private void Start()
    {
        LoadLevel(LevelConfig);
    }

    public void LoadLevel(Level config)
    {
        Debug.Log("Instantiating level map...");
        var tilemap = Instantiate(LevelConfig.LevelDescriptor.Map, tilemapContainer.transform, false);

        var tile =  tilemap.GetTile(new Vector3Int(0, 0, 0));
        Debug.Log(tile.name);
        
        
        LevelLoaded?.Invoke();
    }
}