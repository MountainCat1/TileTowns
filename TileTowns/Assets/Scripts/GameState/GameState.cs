﻿using System;
using System.Collections.Generic;
using Zenject;

public interface IGameState
{
    // Events
    event Action MutationChanged;

    event Action Changed;

    //
    // Data
    float Money { get; }
    float Immigration { get; set; }

    //
    IEnumerable<IGameStateTurnMutation> Mutations { get; }
    int Population { get; set; }
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
    public float Immigration { get; set; }
    public int Population { get; set; }
    

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
        Immigration += mutation.ImmigrationChange ?? 0;
        Population += mutation.PopulationChange ?? 0;
    }
}