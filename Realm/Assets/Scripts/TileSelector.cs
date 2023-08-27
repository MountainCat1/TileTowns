using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    // Events

    public event Action<Vector3Int, TileData> TilePointerEntered;
    public event Action<Vector3Int, TileData> TilePointerClicked;
    
    //
    
    [SerializeField] private LevelManager levelManager;
    [FormerlySerializedAs("sliceMapData")] [SerializeField] private TileMapData tileMapData;
    [SerializeField] private Grid grid;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform tileHighlight;
    
    [SerializeField] private Vector2 mouseOffset;
    
    private Tilemap _tilemap;
    private Camera _camera;

    private Vector3Int _lastCellSelected;
    

    private void Awake()
    {
        inputManager = FindObjectsOfType<InputManager>().Single();
        _camera = Camera.main;
        levelManager.LevelLoaded += () =>
        {
            _tilemap = FindObjectsOfType<Tilemap>().Single();
        };
    }

    private void Start()
    {
        inputManager.PointerMoved += InputManagerOnPointerMoved;
        inputManager.PointerClicked += InputManagerOnPointerClicked;
    }

    private void InputManagerOnPointerClicked(Vector2 pointerPosition)
    {
        var mousePosition = _camera.ScreenToWorldPoint(pointerPosition);
        
        var cell = grid.WorldToCell(new Vector3(mousePosition.x + mouseOffset.x, mousePosition.y +  + mouseOffset.y, 0));

        var tile = _tilemap.GetTile(cell);
        
        if(tile is null)
            return;

        var cellData = tileMapData.GetData(cell);
        
        TilePointerClicked?.Invoke(cell, cellData);
    }

    private void InputManagerOnPointerMoved(Vector2 pointerPosition)
    {
        var mousePosition = _camera.ScreenToWorldPoint(pointerPosition);

        var cell = grid.WorldToCell(new Vector3(mousePosition.x + mouseOffset.x, mousePosition.y +  + mouseOffset.y, 0));

        if(cell == _lastCellSelected)
            return;

        _lastCellSelected = cell;

        tileHighlight.position = grid.GetCellCenterWorld(cell);
        
        var tile = _tilemap.GetTile(cell);
        
        if(tile is null)
            return;
        
        var cellData = tileMapData.GetData(cell);
        
        TilePointerEntered?.Invoke(cell, cellData);
    }
}
