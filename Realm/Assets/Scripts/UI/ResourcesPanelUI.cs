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
        }

        private void GameStateOnChanged()
        {
            moneyDisplay.text = $"{_gameState.Money}";
        }

        private void Update()
        {
            incomeDisplay.text = $"+{_gameState.Changes.Sum(x => x.BuildingIncome)}";
        }
    }
}