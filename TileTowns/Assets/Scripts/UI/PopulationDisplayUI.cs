using TMPro;
using UnityEngine;
using Zenject;

public class PopulationDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] populationDisplays;
    [SerializeField] private TextMeshProUGUI workslotsDisplay;
    [SerializeField] private TextMeshProUGUI housingDisplay;
    [SerializeField] private TextMeshProUGUI assignedWorkersDisplay;

    [SerializeField] private ToolTipSender populationTooltipSender;
    [SerializeField] private ToolTipSender workslotTooltipSender;

    [Inject] private IGameState _gameState;
    [Inject] private ITileMapData _tileMapData;


    [Inject]
    private void Construct()
    {
        _gameState.MutationChanged += UpdateDisplay;
        _gameState.Changed += UpdateDisplay;
    }

    private void Start()
    {
        populationTooltipSender.TooltipDataProvider = () => new TooltipData
        {
            Title = "Population",
            Content = $"Population: {_gameState.Population}\nHousing: {_gameState.Housing}"
        };
        
        workslotTooltipSender.TooltipDataProvider = () => new TooltipData
        {
            Title = "Work Slots",
            Content = $"Assigned workers: {_tileMapData.AssignedWorkers}\nWork slots: {_gameState.WorkSlots}"
        };
    }

    private void UpdateDisplay()
    {
        housingDisplay.text = $"{_gameState.Housing}";
        foreach (var populationDisplay in populationDisplays)
        {
            populationDisplay.text = $"{_gameState.Population}";
        }

        workslotsDisplay.text = $"{_gameState.WorkSlots}";
        assignedWorkersDisplay.text = $"{_tileMapData.AssignedWorkers}";
    }
}