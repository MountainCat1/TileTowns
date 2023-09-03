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
    public GameStateMutation GetMutation();
    public event Action MutationChanged;
}

public interface ITurnManager
{
    // Events
    public event Action<IMutator> MutationHandlerRegistered;
    public event Action TurnStarted;
    public event Action TurnEnded;
    //
    public void RegisterTurnHandler(ITurnHandler turnHandler);
    public void RegisterTurnHandler(IMutator mutator);
    public IReadOnlyCollection<IMutator> MutationHandlers { get; }
}

public class TurnManager : MonoBehaviour, ITurnManager
{
    // Events
    public event Action TurnEnded;
    public event Action TurnStarted;
    public event Action<IMutator> MutationHandlerRegistered;
    //

    [Inject] private IInputManager _inputManager;
    [Inject] private IGameState _gameState;

    private readonly List<ITurnHandler> _turnHandlers = new List<ITurnHandler>();
    private readonly List<IMutator> _turnMutationHandlers = new List<IMutator>();
    
    public IReadOnlyCollection<IMutator> MutationHandlers => _turnMutationHandlers;

    private void OnEnable()
    {
        _inputManager.PlayerPressedSpaceBar += InputManagerOnPlayerPressedSpaceBar;
    }

    private void InputManagerOnPlayerPressedSpaceBar()
    {
        EndTurn();
    }

    public void RegisterTurnHandler(ITurnHandler turnHandler)
    {
        _turnHandlers.Add(turnHandler);
    }
    public void RegisterTurnHandler(IMutator mutator)
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
        RunTurnHandlers();
        
        TurnEnded?.Invoke();
        
        // New Turn Started
        RunTurnHandlers();
        
        TurnStarted?.Invoke();
    }

    private void RunTurnHandlers()
    {
        foreach (var handler in _turnMutationHandlers)
        {
            RefreshHandler(handler);
        }
        
        foreach (var handler in _turnHandlers)
        {
            handler.OnTurn();
        }
    }

    private void RefreshHandler(IMutator handler)
    {
        var mutation = handler.GetMutation();

        _gameState.SetMutation(mutation);
    }
}