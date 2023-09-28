using System;
using System.Collections.Generic;
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
    int Turn { get; }
    // Calculated Data

    public float ImmigrationChange { get; }
    public float BuildingIncome { get; }
    public float MoneyChange { get; }

    //
    IEnumerable<IGameStateTurnMutation> Mutations { get; }
    int Population { get; }
    int WorkSlots { get; }
    void SetMutation(object mutator, IGameStateTurnMutation mutation);
    void SetPersistentModifier(object modifierProvier, IPersistentModifier modifier);
    void ApplyTurnMutations();
    void ApplyMutation(IGameStateMutation mutation);
    void Initialize();
}

public class GameState : IGameState
{
    // Events
    public event Action Changed;

    public event Action MutationChanged;
    //

    public float Money { get; private set; }
    public float Immigration { get; private set; }
    public int Turn => _turnManager.TurnCount;
    public int Population { get; private set; }
    public int Housing => CalculateHousing();
    public int WorkSlots => CalculateWorkSlots();

    // Calculated Data
    public float ImmigrationChange => CalculateImmigrationChange();
    public float BuildingIncome => CalculateBuildingIncome();
    public float MoneyChange => CalculateMoneyChange();

    //
    

    [Inject] private ITurnManager _turnManager;
    
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

    public void Initialize()
    {
        MutationChanged?.Invoke();
        Changed?.Invoke();
    }

    private void OnTurnEnded()
    {
        ApplyTurnMutations();
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

        RoundFloatValues();
        
        Changed?.Invoke();
    }

    public void ApplyTurnMutations()
    {
        foreach (var mutation in Mutations)
        {
            ApplyMutationWithoutNotifying(mutation);
        }

        RoundFloatValues();
        
        Changed?.Invoke();
    }
    
    public void ApplyMutationWithoutNotifying(IGameStateMutation mutation)
    {
        Money += mutation.MoneyChange ?? 0;
        Immigration += mutation.ImmigrationChange ?? 0;
        Population += mutation.PopulationChange ?? 0;

        var immigrationSettings = _gameConfig.ImmigrationSettings;
        if (Immigration >= immigrationSettings.ImmigrationPerPopulation || Immigration <= 0)
        {
            Population += Mathf.FloorToInt(Immigration / immigrationSettings.ImmigrationPerPopulation);
            Immigration %= immigrationSettings.ImmigrationPerPopulation;
        }
    }

    private void RoundFloatValues()
    {
        Money = (float)Math.Round(Money, 2);
        Immigration = (float)Math.Round(Immigration, 2);
    }
    
    private int CalculateHousing()
    {
        int sum = 0;
        foreach (var modifier in PersistentModifiers)
        {
            sum += modifier.Housing;
        }
        return sum;
    }
    
    private int CalculateWorkSlots()
    {
        int sum = 0;
        foreach (var modifier in PersistentModifiers)
        {
            sum += modifier.WorkSlots;
        }
        return sum;
    }
    
    private float CalculateImmigrationChange()
    {
        float sum = 0;
        foreach (var mutation in Mutations)
        {
            sum += mutation.ImmigrationChange ?? 0;
        }
        return sum;
    }

    private float CalculateBuildingIncome()
    {
        float sum = 0;
        foreach (var mutation in Mutations)
        {
            sum += mutation.BuildingIncome;
        }
        return sum;
    }

    private float CalculateMoneyChange()
    {
        return CalculateBuildingIncome();
    }
}