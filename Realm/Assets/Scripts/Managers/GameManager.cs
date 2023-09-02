using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IGameManager
{
    event Action LevelLoaded;
    Tilemap Tilemap { get; }
    LevelConfig LevelConfig { get; set; }
}

public class GameManager : MonoBehaviour, IGameManager
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
