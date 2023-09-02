using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ITurnHandler
{
    public void OnTurn();
}

public interface ITurnManager
{
    // Events
    public event Action TurnCalculated;
    //
    void RegisterTurnHandler(ITurnHandler turnHandler);
    void EndTurn();
}

public class TurnManager : MonoBehaviour, ITurnManager
{
    // Events

    public event Action TurnCalculated;
    
    //
    
    
    [Inject] private IInputManager _inputManager;
    
    private List<ITurnHandler> _turnHandlers;

    private void Awake()
    {
        _turnHandlers = new List<ITurnHandler>();
        _inputManager.PlayerPressedSpaceBar += InputManagerOnPlayerPressedSpaceBar;
    }

    private void InputManagerOnPlayerPressedSpaceBar()
    {
        Debug.Log("New turn! :D");

        EndTurn();
    }
    

    public void RegisterTurnHandler(ITurnHandler turnHandler)
    {
        _turnHandlers.Add(turnHandler);
    }


    public void EndTurn()
    {
        RunTurnHandlers();
        
        TurnCalculated?.Invoke();
    }

    private void RunTurnHandlers()
    {
        foreach (var handler in _turnHandlers)
        {
            handler.OnTurn();
        }
    }
}