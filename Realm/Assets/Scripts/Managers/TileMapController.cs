using UnityEngine;
using Zenject;

public class TileMapController : MonoBehaviour, ITurnHandler
{
    [Inject] private ITileMapData _tileMapData;
    [Inject] private ITurnManager _turnManager;
    [Inject] private IGameState _gameState;

    private void Awake()
    {
        _turnManager.RegisterTurnHandler(this);
        
        _tileMapData.TileAdded += OnTileAdded;
        
        _turnManager.TurnStarted += OnTurnStarted;
    }

    private void OnTurnStarted()
    {
        foreach (var (_, data) in _tileMapData.Data)
        {
            RefreshTileData(data);
        }
    }

    private void OnTileAdded(TileData tileData)
    {
        _tileMapData.TileAdded += (data =>
        {
            data.Changed += () =>
            {
                RefreshTileData(data);
            };
        });
    }

    public void OnTurn()
    {
        foreach (var (_, data) in _tileMapData.Data)
        {
            RefreshTileData(data);
        }
    }

    private void RefreshTileData(TileData tileData)
    {
        var change = tileData.GetChange();

        _gameState.SetChange(change);
    }
}