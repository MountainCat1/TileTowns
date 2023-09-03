using System;
using System.Collections.Generic;
using Zenject;

public interface IGameState
{
    decimal Money { get; set; }
    IEnumerable<GameStateChange> Changes { get; }
    event Action Changed;
    void SetChange(GameStateChange change);
    void ApplyChanges();
}

public class GameState : GameStateData, IGameState
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
        
        turnManager.TurnEnded += TurnManagerOnTurnCalculated;
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
    
    public void ApplyChanges()
    {
        foreach (var change in Changes)
        {
            ApplyChange(change);
        }
        
        Changed?.Invoke();
    }

    public void ApplyChange(GameStateChange change)
    {
        Money += change.BuildingIncome;
    }
}