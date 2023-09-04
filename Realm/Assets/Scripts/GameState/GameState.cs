using System;
using System.Collections.Generic;
using Zenject;

public interface IGameState
{
    // Events
    event Action MutationChanged;

    event Action Changed;

    //
    // Data
    public decimal Money { get; }

    //
    IEnumerable<IGameStateTurnMutation> Mutations { get; }
    void SetMutation(IGameStateTurnMutation mutation);
    void ApplyMutations();
    void ForceApplyMutation(IGameStateMutation mutation);
}

public class GameState : IGameState
{
    // Events
    public event Action Changed;

    public event Action MutationChanged;
    //

    public decimal Money { get; private set; }

    public IEnumerable<IGameStateTurnMutation> Mutations => _mutations.Values;
    private Dictionary<object, IGameStateTurnMutation> _mutations;

    [Inject]
    public GameState(ITurnManager turnManager)
    {
        _mutations = new Dictionary<object, IGameStateTurnMutation>();

        turnManager.TurnEnded += OnTurnEnded;
    }

    private void OnTurnEnded()
    {
        ApplyMutations();

        _mutations.Clear();
    }

    public void SetMutation(IGameStateTurnMutation mutation)
    {
        // Adds new mutation, if exists mutation with specified mutator exists - overrides it 
        _mutations[mutation.Mutator] = mutation;
        MutationChanged?.Invoke();
    }

    public void ForceApplyMutation(IGameStateMutation mutation)
    {
        ApplyMutation(mutation);

        Changed?.Invoke();
    }

    public void ApplyMutations()
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