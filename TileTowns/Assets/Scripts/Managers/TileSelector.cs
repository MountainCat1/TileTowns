using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface ITileSelector
{
    event Action<Vector3Int, TileData> TilePointerEntered;
    event Action<Vector3Int, TileData> TilePointerClicked;
}

public class TileSelector : MonoBehaviour, ITileSelector
{
    // Events

    public event Action<Vector3Int, TileData> TilePointerEntered;
    public event Action<Vector3Int, TileData> TilePointerClicked;
    
    //
    
    [Inject] private IGameManager _gameManager;
    [Inject] private IInputManager _inputManager;
    [Inject] private ITileMapData _tileMapData;
    
    [SerializeField] private Grid grid;
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private Tilemap highlightGrid;
    
    private Tilemap _tilemap;
    private Camera _camera;

    private Vector3Int _lastCellSelected;
    

    private void Awake()
    {
        _inputManager = FindObjectsOfType<InputManager>().Single();
        _camera = Camera.main;
        _gameManager.LevelLoaded += () =>
        {
            _tilemap = _gameManager.Tilemap;
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
        
        if(tile is null)
            return;

        var cellData = _tileMapData.GetData(cell);
        
        TilePointerClicked?.Invoke(cell, cellData);
    }

    private void InputManagerOnPointerMoved(Vector2 pointerPosition)
    {
        var mousePosition = _camera.ScreenToWorldPoint(pointerPosition);

        var cell = grid.WorldToCell(new Vector3(mousePosition.x, mousePosition.y, 0));
        
        if(cell == _lastCellSelected)
            return;

        MoveTileHighlightTo(_lastCellSelected, cell);
        
        _lastCellSelected = cell;
        
        var tile = _tilemap.GetTile(cell);
        
        if(tile is null)
            return;
        
        var cellData = _tileMapData.GetData(cell);
        
        TilePointerEntered?.Invoke(cell, cellData);
    }

    private void MoveTileHighlightTo(Vector3Int lastHighlightedCell, Vector3Int cell)
    {
        highlightGrid.SetTile(lastHighlightedCell, null);
        highlightGrid.SetTile(cell, highlightTile);
    }
}
