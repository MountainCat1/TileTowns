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

    public Tilemap Tilemap { get; private set; }
    
    [field: SerializeField] public LevelConfig LevelConfig { get; set; }
    [SerializeField] private Grid tilemapContainer;

    private void Start()
    {
        LoadLevel(LevelConfig);
    }

    public void LoadLevel(LevelConfig config)
    {
        Debug.Log("Instantiating level map...");
        Tilemap = Instantiate(LevelConfig.LevelDescriptor.Map, tilemapContainer.transform, false);

        LevelLoaded?.Invoke();
    }
}
