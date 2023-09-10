using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

public class PostprocessingController : MonoBehaviour
{
    [Inject] private IPlayerController _playerController;

    [SerializeField] private Volume volume;

    [SerializeField] private VolumeProfile defaultProfile;
    [SerializeField] private VolumeProfile popSelectionProfile;
    
    private void Start()
    {
        _playerController.PlayerModeSet += OnPlayerModeSet;
    }

    private void OnPlayerModeSet(PlayerMode obj)
    {
        switch (obj)
        {
            case PlayerMode.Default:
                volume.profile = defaultProfile;
                break;
            case PlayerMode.Building:
                volume.profile = defaultProfile;
                break;
            case PlayerMode.PopulationManaging:
                volume.profile = popSelectionProfile;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
        }
    }
}
