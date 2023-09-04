using System;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ResourcesPanelUI : MonoBehaviour
    {
        [Inject] private IGameState _gameState;
        
        [SerializeField] private TextMeshProUGUI moneyDisplay;
        [SerializeField] private TextMeshProUGUI incomeDisplay;

        private void OnEnable()
        {
            _gameState.Changed += GameStateOnChanged;
            _gameState.MutationChanged += GameStateMutationChanged;
        }

        private void GameStateMutationChanged() {
            incomeDisplay.text = $"+{_gameState.Mutations.Sum(x => x.MoneyChange)}";
        }

        private void GameStateOnChanged()
        {
            moneyDisplay.text = $"{_gameState.Money}";
        }
    }
}