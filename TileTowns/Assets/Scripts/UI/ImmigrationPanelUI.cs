using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ImmigrationPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI changeDisplay;
        [SerializeField] private TextMeshProUGUI progressTextDisplay;
        [SerializeField] private Slider progressSliderDisplay;

        [Inject] private IGameState _gameState;

        [Inject]
        void Construct()
        {
            _gameState.Changed += UpdateData;
            _gameState.MutationChanged += UpdateMutationData;
        }

        private void UpdateMutationData()
        {
            var immigrationChange = _gameState.Mutations.Sum(x => x.ImmigrationChange);
            
            changeDisplay.text = $"+ {immigrationChange}%";
        }

        private void UpdateData()
        {
            progressSliderDisplay.value = _gameState.Immigration;
            progressTextDisplay.text = $"{_gameState.Immigration}%";
        }
    }
}