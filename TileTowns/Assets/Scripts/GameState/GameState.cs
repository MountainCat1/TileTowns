using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public interface IGameState
{
    // Events
    event Action MutationChanged;

    event Action Changed;

    //
    // Data
    float Money { get; }
    float Immigration { get; }
    int Housing { get; }

    //
    IEnumerable<IGameStateTurnMutation> Mutations { get; }
    int Population { get; }
    int WorkSlots { get; }
    void SetMutation(object mutator, IGameStateTurnMutation mutation);
    void SetPersistentModifier(object modifierProvier, IPersistentModifier modifier);
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
    public int Housing => CalculateHousing();
    public int WorkSlots => CalculateWorkSlots();

    public int Population { get; set; }

    [Inject] private IGameConfig _gameConfig;
    
    public IEnumerable<IGameStateTurnMutation> Mutations => _mutations.Values;
    private Dictionary<object, IGameStateTurnMutation> _mutations;
    
    private IEnumerable<IPersistentModifier> PersistentModifiers => _persistentModifiers.Values;
    private Dictionary<object, IPersistentModifier> _persistentModifiers;

    [Inject]
    public GameState(ITurnManager turnManager)
    {
        _mutations = new Dictionary<object, IGameStateTurnMutation>();
        _persistentModifiers = new Dictionary<object, IPersistentModifier>();

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
    
    public void SetPersistentModifier(object modifierProvier, IPersistentModifier modifier)
    {
        // Adds new mutation, if exists mutation with specified mutator exists - overrides it 
        _persistentModifiers[modifierProvier] = modifier;
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

        if (Immigration >= _gameConfig.ImmigrationPerPopulation)
        {
            Population += Mathf.FloorToInt(Immigration / _gameConfig.ImmigrationPerPopulation);
            Immigration %= _gameConfig.ImmigrationPerPopulation;
        }
    }
    
    private int CalculateHousing()
    {
        return PersistentModifiers.Sum(x => x.Housing);
    }
    
    private int CalculateWorkSlots()
    {
        return PersistentModifiers.Sum(x => x.WorkSlots);
    }
}