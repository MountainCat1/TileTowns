using System;
using UnityEngine;
using Zenject;

public enum PlayerMode
{
    Default, Building, PopulationManaging
}

public class PlayerController : MonoBehaviour
{
    [Inject] private IInputManager _inputManager;

    private void OnEnable()
    {
        _inputManager.PlayerPressedTab += SwitchBetweenModes;
    }

    private void SwitchBetweenModes()
    {
        // TODO
        Debug.LogError("AAA this is NOT implemented! WHY?!!!!! 💀💀💀");
    }
}