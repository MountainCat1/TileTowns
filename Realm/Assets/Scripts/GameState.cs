using System;
using System.Collections.Generic;
using Zenject;

public interface IGameState
{
    decimal Money { get; set; }
    IEnumerable<GameStateChange> Changes { get; }
    event Action Changed;
    void SetChange(GameStateChange change);
}

public class GameState : IGameState
{
    // Events
    public event Action Changed;
    //
    
    public decimal Money { get; set; }

    public IEnumerable<GameStateChange> Changes => _changes.Values;
    private Dictionary<object, GameStateChange> _changes;

    [Inject]
    public GameState(ITurnManager turnManager)
    {
        _changes = new Dictionary<object, GameStateChange>();
        
        turnManager.TurnCalculated += TurnManagerOnTurnCalculated;
    }

    private void TurnManagerOnTurnCalculated()
    {
        ApplyChanges();

        _changes.Clear();
    }


    public void SetChange(GameStateChange change)
    {
        _changes[change.Mutator] = change;
    }
    
    private void ApplyChanges()
    {
        foreach (var change in Changes)
        {
            ApplyChange(change);
        }
        
        Changed?.Invoke();
    }
    
    private void ApplyChange(GameStateChange change)
    {
        Money += change.BuildingIncome;
    }
}