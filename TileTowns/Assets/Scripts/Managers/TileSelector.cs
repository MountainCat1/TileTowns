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
    [Inject] private ITileMapData tileMapData;
    
    [SerializeField] private Grid grid;
    [SerializeField] private TileBase highlightTile;
    [SerializeField] private Tilemap highlightGrid;
    
    private Tilemap _tilemap;
    [Inject] private Camera _camera;

    private Vector3Int _lastCellSelected;
    
    [Inject]
    private void Construct()
    {
        _gameManager.LevelLoaded += OnLevelLoaded;
        _inputManager.PointerMoved += OnPointerMoved;
        _inputManager.PointerClicked += OnPointerClicked;
        _inputManager.PointerSecondaryClicked += OnPointerSecondaryClicked;
        
        _gameManager.LevelEnded += OnLevelEnded;
        _gameManager.GameStageChanged += OnGameStageChanged;
    }

    private void OnGameStageChanged(GameStage stage)
    {
        if(stage is GameStage.Ended or GameStage.Pause)
            RemoveSelector();
    }

    private void OnLevelEnded(IGameResult gameResult)
    {
        _inputManager.PointerMoved -= OnPointerMoved;
        _inputManager.PointerClicked -= OnPointerClicked;
        _inputManager.PointerSecondaryClicked -= OnPointerSecondaryClicked;
        
        RemoveSelector();
    }

    public void OnDestroy()
    {
        _gameManager.LevelLoaded -= OnLevelLoaded;
        _inputManager.PointerMoved -= OnPointerMoved;
        _inputManager.PointerClicked -= OnPointerClicked;
        _inputManager.PointerSecondaryClicked -= OnPointerSecondaryClicked;
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
        if(_gameManager.GameStage is GameStage.Pause or GameStage.Ended)
            return;
        
        var cell = PointerPositionToCell(pointerPosition);

        if (cell == _lastCellSelected)
            return;

        // If the cell is not in the tilemap, don't show up the highlight
        if (tileMapData.Data.TryGetValue((Vector2Int)cell, out var data))
        {
            MoveTileHighlightTo(_lastCellSelected, cell);
            
        }
        else
        {
            RemoveSelector();
        }
        
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
    
    private void RemoveSelector()
    {
        highlightGrid.SetTile(_lastCellSelected, null);
    }
}
