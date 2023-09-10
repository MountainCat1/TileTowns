using System;
using UnityEngine;
using Zenject;

public enum PlayerMode
{
    Default,
    Building,
    PopulationManaging
}

public interface IPlayerController
{
    event Action<PlayerMode> PlayerModeSet;
    void SetPlayerMode(PlayerMode newPlayerMode);
    PlayerMode PlayerMode { get; }
}

public class PlayerController : MonoBehaviour, IPlayerController
{
    // Events

    public event Action<PlayerMode> PlayerModeSet; 

    //
    
    [Inject] private IInputManager _inputManager;
    [Inject] private ITurnManager _turnManager;

    public PlayerMode PlayerMode { get; private set; }

    private void OnEnable()
    {
        _inputManager.PlayerPressedTab += SwitchBetweenModes;
        _inputManager.PlayerPressedSpaceBar += OnPlayerPressedSpaceBar;
    }

    private void OnPlayerPressedSpaceBar()
    {
        _turnManager.EndTurn();
    }

    public void SetPlayerMode(PlayerMode newPlayerMode)
    {
        Debug.Log($"Switching to {newPlayerMode} mode...");
        PlayerMode = newPlayerMode;
        
        PlayerModeSet?.Invoke(PlayerMode);
    }


    private void SwitchBetweenModes()
    {
        if (PlayerMode == PlayerMode.Default)
        {
            SetPlayerMode(PlayerMode.PopulationManaging);
            return;
        }

        SetPlayerMode(PlayerMode.Default);
    }
}