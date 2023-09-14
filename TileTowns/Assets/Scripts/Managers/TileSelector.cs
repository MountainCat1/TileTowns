using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

public interface ITileSelector
{
    event Action<Vector3Int, TileData> TilePointerEntered;
    event Action<Vector3Int, TileData> TilePointerClicked;
    event Action<Vector3Int, TileData> TilePointerSecondaryClicked;
}

public class TileSelector : MonoBehaviour, ITileSelector
{
    #region Events
    public event Action<Vector3Int, TileData> TilePointerEntered;
    public event Action<Vector3Int, TileData> TilePointerClicked;
    public event Action<Vector3Int, TileData> TilePointerSecondaryClicked;
    #endregion

    [Inject] private IGameManager _gameManager;
    [Inject] private ITileMapData _tileMapData;
    [Inject] private IInputManager _inputManager;
    
    [SerializeField] private Grid grid;
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private Tilemap highlightGrid;
    [SerializeField] private CameraMovement cameraMovement;
    
    private Tilemap _tilemap;
    private Camera _camera;

    private Vector3Int _lastCellSelected;
    

    private void Awake()
    {
        _camera = Camera.main;

        if (_camera == null)
        {
            Debug.LogError("Main Camera not found.");
            return;
        }

        _gameManager.LevelLoaded += OnLevelLoaded;
    }

    private void Start()
    {
        _inputManager.PointerMoved += OnPointerMoved;
        _inputManager.PointerClicked += OnPointerClicked;
        _inputManager.PointerSecondaryClicked += OnPointerSecondaryClicked;
    }

    private void OnLevelLoaded()
    {
        _tilemap = _gameManager.Tilemap;
    }

    private void OnPointerSecondaryClicked(Vector2 pointerPosition)
    {
        var cell = PointerPositionToCell(pointerPosition);

        var tile = _tilemap.GetTile(cell);

        if (tile == null)
            return;

        var cellData = _tileMapData.GetData((Vector2Int)cell);
        TilePointerSecondaryClicked?.Invoke(cell, cellData);
    }
    
    private void OnPointerClicked(Vector2 pointerPosition)
    {
        var cell = PointerPositionToCell(pointerPosition);

        var tile = _tilemap.GetTile(cell);

        if (tile == null)
            return;

        var cellData = _tileMapData.GetData((Vector2Int)cell);
        TilePointerClicked?.Invoke(cell, cellData);
    }

    private void OnPointerMoved(Vector2 pointerPosition)
    {
        var cell = PointerPositionToCell(pointerPosition);

        if (cell == _lastCellSelected)
            return;

        MoveTileHighlightTo(_lastCellSelected, cell);
        _lastCellSelected = cell;

        var cellData = _tileMapData.GetData((Vector2Int)cell);
        TilePointerEntered?.Invoke(cell, cellData);
    }

    private Vector3Int PointerPositionToCell(Vector2 pointerPosition)
    {
        var worldPosition = _camera.ScreenToWorldPoint(pointerPosition);
        return grid.WorldToCell(new Vector3(worldPosition.x, worldPosition.y, 0));
    }

    private void MoveTileHighlightTo(Vector3Int lastHighlightedCell, Vector3Int cell)
    {
        highlightGrid.SetTile(lastHighlightedCell, null);
        highlightGrid.SetTile(cell, highlightTile);
    }
}
