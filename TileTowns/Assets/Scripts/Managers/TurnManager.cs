using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ITurnHandler
{
    public void OnTurn();
}

public interface IMutator
{
    public IGameStateTurnMutation GetMutation();
    public IPersistentModifier GetPersistentModifier();
    public event Action MutationChanged;
}

public interface ITurnManager
{
    // Events
    public event Action<IMutator> MutationHandlerRegistered;
    public event Action TurnStarted;
    public event Action TurnEnded;
    //
    public void EndTurn();
    public void RegisterTurnHandler(ITurnHandler turnHandler);
    public void RegisterMutator(IMutator mutator);
    public IReadOnlyCollection<IMutator> MutationHandlers { get; }
    int TurnCount { get; }
}

public class TurnManager : MonoBehaviour, ITurnManager
{
    // Events
    public event Action TurnEnded;
    public event Action TurnStarted;
    public event Action<IMutator> MutationHandlerRegistered;
    //
    
    [Inject] private IGameState _gameState;
    
    public int TurnCount { get; private set; }

    private readonly List<ITurnHandler> _turnHandlers = new List<ITurnHandler>();
    private readonly List<IMutator> _turnMutationHandlers = new List<IMutator>();
    
    public IReadOnlyCollection<IMutator> MutationHandlers => _turnMutationHandlers;
    
    public void RegisterTurnHandler(ITurnHandler turnHandler)
    {
        _turnHandlers.Add(turnHandler);
    }
    public void RegisterMutator(IMutator mutator)
    {
        _turnMutationHandlers.Add(mutator);
        mutator.MutationChanged += () =>
        {
            RefreshHandler(mutator);
        };
    }

    public void EndTurn()
    {
        // End turn
        // RunTurnHandlers();
        
        foreach (var handler in _turnHandlers)
        {
            handler.OnTurn();
        }
        
        TurnEnded?.Invoke();
        
        // // New Turn Started
        // RunTurnHandlers();

        TurnCount++;
        
        TurnStarted?.Invoke();
    }

    private void RunTurnHandlers()
    {
        foreach (var handler in _turnMutationHandlers)
        {
            RefreshHandler(handler);
        }
    }

    private void RefreshHandler(IMutator mutator)
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        Debug.Log($"Refreshing mutator: {mutator}");
        
        var mutation = mutator.GetMutation();

        _gameState.SetMutation(mutator, mutation);
        
        var persistentModifier = mutator.GetPersistentModifier();
        
        _gameState.SetPersistentModifier(mutator, persistentModifier);
    }
}