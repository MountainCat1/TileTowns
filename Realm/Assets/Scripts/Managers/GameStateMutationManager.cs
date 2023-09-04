﻿using Zenject;

public interface IGameStateManager
{
}

public class GameStateMutationManager : IGameStateManager
{
    [Inject] private IGameState _gameState;
    [Inject] private ITurnManager _turnManager;

    [Inject]
    private void Construct()
    {
        _turnManager.TurnEnded += OnTurnEnded;
        _turnManager.TurnStarted += OnTurnStarted;
        _turnManager.MutationHandlerRegistered += OnMutationHandlerRegistered;
    }

    private void OnMutationHandlerRegistered(IMutator mutator)
    {
        mutator.MutationChanged += () =>
        {
            RefreshMutator(mutator);
        };
    }
    
    private void OnTurnStarted()
    {
        RefreshAllMutators();
    }

    private void OnTurnEnded()
    {
        _gameState.ApplyMutations();
    }

    private void RefreshAllMutators()
    {
        foreach (var mutator in _turnManager.MutationHandlers)
        {
            RefreshMutator(mutator);
        }
    }
    private void RefreshMutator(IMutator mutator)
    {
        var mutation = mutator.GetMutation();
        
        _gameState.SetMutation(mutation);
    }
}