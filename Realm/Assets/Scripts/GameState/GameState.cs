using System;
using System.Collections.Generic;
using Zenject;

public interface IGameState : IGameStateData
{
    // Events
    event Action MutationChanged;
    event Action Changed;
    //
    // Data
    public decimal Money { get; set; }
    
    //
    IEnumerable<IGameStateMutation> Mutations { get; }
    void SetMutation(GameStateMutation mutation);
    void ApplyChanges();
}

public class GameState : GameStateData, IGameState
{
    // Events
    public event Action Changed;
    public event Action MutationChanged;
    //
    
    public decimal Money { get; set; }

    public IEnumerable<IGameStateMutation> Mutations => _mutations.Values;
    private Dictionary<object, IGameStateMutation> _mutations;

    [Inject]
    public GameState(ITurnManager turnManager)
    {
        _mutations = new Dictionary<object, IGameStateMutation>();
        
        turnManager.TurnEnded += TurnManagerOnTurnCalculated;
    }

    private void TurnManagerOnTurnCalculated()
    {
        ApplyChanges();

        _mutations.Clear();
    }


    public void SetMutation(GameStateMutation mutation)
    {
        _mutations[mutation.Mutator] = mutation;
        MutationChanged?.Invoke();
    }
    
    public void ApplyChanges()
    {
        foreach (var mutation in Mutations)
        {
            ApplyChange(mutation);
        }
        
        Changed?.Invoke();
    }

    public void ApplyChange(IGameStateMutation mutation)
    {
        Money += mutation.BuildingIncome;
    }
}