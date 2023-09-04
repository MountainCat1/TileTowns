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
    public decimal Money { get; }

    //
    IEnumerable<IGameStateMutation> Mutations { get; }
    void SetMutation(IGameStateTurnMutation mutation);
    void ApplyChanges();
    void ForceApplyMutation(IGameStateMutation mutation);
}

public class GameState : GameStateData, IGameState
{
    // Events
    public event Action Changed;

    public event Action MutationChanged;
    //

    public decimal Money { get; private set; }

    public IEnumerable<IGameStateMutation> Mutations => _mutations.Values;
    private Dictionary<object, IGameStateMutation> _mutations;

    [Inject]
    public GameState(ITurnManager turnManager)
    {
        _mutations = new Dictionary<object, IGameStateMutation>();

        turnManager.TurnEnded += OnTurnCalculated;
    }

    private void OnTurnCalculated()
    {
        ApplyChanges();

        _mutations.Clear();
    }


    public void SetMutation(IGameStateTurnMutation mutation)
    {
        _mutations[mutation.Mutator] = mutation;
        MutationChanged?.Invoke();
    }

    public void ForceApplyMutation(IGameStateMutation mutation)
    {
        ApplyMutation(mutation);

        Changed?.Invoke();
    }

    public void ApplyChanges()
    {
        foreach (var mutation in Mutations)
        {
            ApplyMutation(mutation);
        }

        Changed?.Invoke();
    }

    public void ApplyMutation(IGameStateMutation mutation)
    {
        Money += mutation.MoneyChange ?? 0;
    }
}