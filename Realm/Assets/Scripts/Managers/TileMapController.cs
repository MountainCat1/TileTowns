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
    }
    
    public void OnTurn()
    {
        foreach (var (position, data) in _tileMapData.Data)
        {
            var change = new GameStateChange();
            
            data.OnTurn(position, change);

            _gameState.AddChage(change);
        }
    }
}