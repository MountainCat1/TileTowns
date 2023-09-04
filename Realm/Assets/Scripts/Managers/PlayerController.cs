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
}

public class PlayerController : MonoBehaviour, IPlayerController
{
    // Events

    public event Action<PlayerMode> PlayerModeSet; 

    //
    
    [Inject] private IInputManager _inputManager;

    private PlayerMode _playerMode;

    private void OnEnable()
    {
        _inputManager.PlayerPressedTab += SwitchBetweenModes;
    }

    public void SetPlayerMode(PlayerMode newPlayerMode)
    {
        Debug.Log($"Switching to {newPlayerMode} mode...");
        _playerMode = newPlayerMode;
        
        PlayerModeSet?.Invoke(_playerMode);
    }


    private void SwitchBetweenModes()
    {
        if (_playerMode == PlayerMode.Default)
        {
            SetPlayerMode(PlayerMode.PopulationManaging);
            return;
        }

        SetPlayerMode(PlayerMode.Default);
    }
}