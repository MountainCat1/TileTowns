using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public Level LevelConfig { get; set; } 
    
    [SerializeField] private Grid tilemapContainer;

    private void Start()
    {
        LoadLevel(LevelConfig);
    }

    public void LoadLevel(Level config)
    {
        Debug.Log("Instantiating level map...");
        var tilemap = Instantiate(LevelConfig.LevelDescriptor.Map, tilemapContainer.transform, true);
    }
}