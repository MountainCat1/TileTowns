using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    
    // Events

    public event Action<Vector3Int, TileBase> TilePointerEntered;
    public event Action<Vector3Int, TileBase> TilePointerClicked;
    
    //
    
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Grid grid;
    

    private InputManager _inputManager;
    private Tilemap _tilemap;
    private Camera _camera;

    private Vector3Int _lastCellSelected;
    

    private void Awake()
    {
        _inputManager = FindObjectsOfType<InputManager>().Single();
        _camera = Camera.main;
        levelManager.LevelLoaded += () =>
        {
            _tilemap = FindObjectsOfType<Tilemap>().Single();
        };
    }

    private void Start()
    {
        _inputManager.PointerMoved += InputManagerOnPointerMoved;
        _inputManager.PointerClicked += InputManagerOnPointerClicked;
    }

    private void InputManagerOnPointerClicked(Vector2 pointerPosition)
    {
        var mousePosition = _camera.ScreenToWorldPoint(pointerPosition);
        
        var cell = grid.WorldToCell(new Vector3(mousePosition.x, mousePosition.y, 0));

        var tile = _tilemap.GetTile(cell);
        
        TilePointerClicked?.Invoke(cell, tile);
    }

    private void InputManagerOnPointerMoved(Vector2 pointerPosition)
    {
        var mousePosition = _camera.ScreenToWorldPoint(pointerPosition);
        
        var cell = grid.WorldToCell(new Vector3(mousePosition.x, mousePosition.y, 0));

        if(cell == _lastCellSelected)
            return;

        _lastCellSelected = cell;
        
        var tile = _tilemap.GetTile(cell);
        
        TilePointerEntered?.Invoke(cell, tile);
    }
}
