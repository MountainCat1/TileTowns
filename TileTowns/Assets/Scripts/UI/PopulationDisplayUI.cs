using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PopulationDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI populationDisplay;
    [SerializeField] private TextMeshProUGUI workslotsDisplay;
    [SerializeField] private TextMeshProUGUI housingDisplay;

    [Inject] private IGameState _gameState;


    [Inject]
    private void Construct()
    {
        _gameState.MutationChanged += UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        housingDisplay.text = $"{_gameState.Housing}";
    }
}
