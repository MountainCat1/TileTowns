using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface ILevelManager
{
    event Action LevelLoaded;
    Tilemap Tilemap { get; }
    LevelConfig LevelConfig { get; set; }
}

public class LevelManager : MonoBehaviour, ILevelManager
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
