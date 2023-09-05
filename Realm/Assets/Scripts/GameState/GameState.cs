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
    public float Money { get; }

    //
    IEnumerable<IGameStateTurnMutation> Mutations { get; }
    void SetMutation(object mutator, IGameStateTurnMutation mutation);
    void ApplyTurnMutations();
    void ApplyMutation(IGameStateMutation mutation);
}

public class GameState : IGameState
{
    // Events
    public event Action Changed;

    public event Action MutationChanged;
    //

    public float Money { get; private set; }

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
        ApplyTurnMutations();

        _mutations.Clear();
    }

    public void SetMutation(object mutator, IGameStateTurnMutation mutation)
    {
        // Adds new mutation, if exists mutation with specified mutator exists - overrides it 
        _mutations[mutator] = mutation;
        MutationChanged?.Invoke();
    }

    public void ApplyMutation(IGameStateMutation mutation)
    {
        ApplyMutationWithoutNotifying(mutation);

        Changed?.Invoke();
    }

    public void ApplyTurnMutations()
    {
        foreach (var mutation in Mutations)
        {
            ApplyMutationWithoutNotifying(mutation);
        }

        Changed?.Invoke();
    }

    public void ApplyMutationWithoutNotifying(IGameStateMutation mutation)
    {
        Money += mutation.MoneyChange ?? 0;
    }
}