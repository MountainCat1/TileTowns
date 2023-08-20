using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    [field: SerializeField] public TileBase SelectedTile { get; set; }
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Grid grid;
    

    private InputManager _inputManager;
    private Tilemap _tilemap;
    private Camera _camera;
    

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
    }

    private void InputManagerOnPointerMoved(Vector2 pointerPosition)
    {
        var mousePosition = _camera.ScreenToWorldPoint(pointerPosition);
        
        var cell = grid.WorldToCell(new Vector3(mousePosition.x, mousePosition.y, 0));

        Debug.Log(cell);
        SelectedTile = _tilemap.GetTile(cell);
    }
}
