using UnityEngine;
using Zenject;

public class TileMapController : MonoBehaviour, ITurnHandler
{
    [Inject] private ITileMapData _tileMapData;
    [Inject] private ITurnManager _turnManager;
    [Inject] private InputManager _inputManager;

    private void Awake()
    {
        _turnManager.RegisterTurnHandler(this);
    }

    public void OnTurn()
    {
        foreach (var (position, data) in _tileMapData.Data)
        {
            data.OnTurn(position);
        }
    }
}