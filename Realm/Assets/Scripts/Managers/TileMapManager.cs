using System;
using UnityEngine;
using Zenject;

public class TileMapManager : MonoBehaviour
{
    [Inject] private ITileMapData _tileMapData;
    [Inject] private ITurnManager _turnManager;

    private void Awake()
    {
        _tileMapData.TileAdded += OnTileAdded;
    }

    private void OnTileAdded(TileData tileData)
    {
        _turnManager.RegisterTurnHandler(tileData);
    }
}