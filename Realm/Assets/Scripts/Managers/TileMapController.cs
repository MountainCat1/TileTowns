using System;
using UnityEngine;
using Zenject;

public class TileMapController : MonoBehaviour
{
    [Inject] private ITileMapData _tileMapData;
    [Inject] private ITurnManager _turnManager;
    [Inject] private IGameState _gameState;

    private void Awake()
    {
        _tileMapData.TileAdded += OnTileAdded;
    }

    private void OnTileAdded(TileData tileData)
    {
        _turnManager.RegisterTurnHandler(tileData);
    }
}