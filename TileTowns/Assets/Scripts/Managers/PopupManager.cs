using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public interface IPopupManager
{
}

public class PopupManager : MonoBehaviour, IPopupManager
{
    [Inject] private ITurnManager _turnManager;
    [Inject] private IGameState _gameState;
    [Inject] private IGameManager _gameManager;

    [SerializeField] private Transform popupContainer;
    [SerializeField] private PopupDescriptor popupPrefab;
    
    [SerializeField] private float timeToDestroyPopup = 1f;
    [SerializeField] private Vector2 popupOffset = new Vector2(0, 0.5f);
    private const float DepthOffset = 5f;

    private void Start()
    {
        _turnManager.TurnStarted += OnTurnStarted;
    }

    private void OnTurnStarted()
    {
        DisplayTileMutations();
    }
    


    private void DisplayTileMutations()
    {
        var tilesWithMutations = GetTilesWithMutations();

        foreach (var (tile, mutation) in tilesWithMutations)
        {
            DisplayTileMutation(tile, mutation);
        }
    }

    private IEnumerable<(ITileData tile, IGameStateTurnMutation mutation)> GetTilesWithMutations()
    {
        return _gameState.Mutators
            .Where(pair => pair.Key is ITileData)
            .Select(pair => (tile: (ITileData) pair.Key, mutation: pair.Value))
            .Where(tuple => tuple.mutation.BuildingIncome != 0);
    }

    private void DisplayTileMutation(ITileData tile, IGameStateTurnMutation mutation)
    {
        var incomeText = FormatIncome(mutation.BuildingIncome);
        var incomeColor = DetermineIncomeColor(mutation.BuildingIncome);
        var popupPosition = CalculatePopupPosition(tile.Position);
        
        SpawnPopup(incomeText, popupPosition, incomeColor);
    }

    private string FormatIncome(float income)
    {
        return income > 0 ? $"+{income}" : $"{income}";
    }

    private Color DetermineIncomeColor(float income)
    {
        return income > 0 ? Color.green : Color.red;
    }

    private Vector3 CalculatePopupPosition(Vector2Int tilePosition)
    {
        var worldPosition = _gameManager.Tilemap.CellToWorld((Vector3Int) tilePosition);
        return worldPosition + new Vector3(0, 0, DepthOffset) + (Vector3)popupOffset;
    }

    public void SpawnPopup(string text, Vector3 position, Color color)
    {
        var popup = Instantiate(popupPrefab, popupContainer);
        popup.transform.position = position;
        popup.Text.text = text;
        popup.Text.color = color;
        
        Destroy(popup.gameObject, timeToDestroyPopup);
    }
}