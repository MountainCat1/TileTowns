using System;
using System.Collections.Generic;
using Zenject;

public interface IGameState
{
    decimal Money { get; set; }
    event Action Changed;
    void AddChage(GameStateChange change);
}

public class GameState : IGameState
{
    // Events
    public event Action Changed;
    //
    
    public decimal Money { get; set; }

    private List<GameStateChange> _changes;

    [Inject]
    public GameState(ITurnManager turnManager)
    {
        _changes = new List<GameStateChange>();
        
        turnManager.TurnCalculated += TurnManagerOnTurnCalculated;
    }

    private void TurnManagerOnTurnCalculated()
    {
        ApplyChanges(_changes);

        _changes = new List<GameStateChange>();
    }


    public void AddChage(GameStateChange change)
    {
        _changes.Add(change);
    }
    
    private void ApplyChanges(IEnumerable<GameStateChange> changes)
    {
        foreach (var change in changes)
        {
            ApplyChange(change);
        }
        
        Changed?.Invoke();
    }
    
    private void ApplyChange(GameStateChange change)
    {
        Money += change.Income;
    }
}