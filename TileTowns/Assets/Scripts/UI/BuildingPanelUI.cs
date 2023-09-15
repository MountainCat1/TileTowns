using System.Collections.Generic;
using Buildings.Extensions;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BuildingPanelUI : MonoBehaviour
    {
        [Inject] private IBuildingController _buildingController;
        [Inject] private IGameManager _gameManager;
        [Inject] private DiContainer _container;

        [SerializeField] private Transform buildingChoiceContainer;

        [SerializeField] private BuildingEntryUI buildingEntryPrefab;

        private List<BuildingEntryUI> _buildingEntries; 

        private void OnEnable()
        {
            _gameManager.LevelLoaded += GameManagerOnGameLoaded;
            _buildingController.BuildingDeselected += OnBuildingDeselected;
        }

        private void OnBuildingDeselected()
        {
            foreach (var buildingEntryUI in _buildingEntries)
                buildingEntryUI.ShowAsDeselected();
        }

        private void OnDisable()
        {
            _gameManager.LevelLoaded -= GameManagerOnGameLoaded;
        }


        private void GameManagerOnGameLoaded()
        {
            LoadBuildingData();
        }

        private void LoadBuildingData()
        {
            _buildingEntries = new List<BuildingEntryUI>();
            
            foreach (var buildingData in _gameManager.LevelConfig.BuildingSet)
            {
                var buildingEntry = CreateBuildingEntries(buildingData);
                
                _buildingEntries.Add(buildingEntry);
            }
        }

        private BuildingEntryUI CreateBuildingEntries(Building building)
        {
            var createdButtonGo = _container.InstantiatePrefab(buildingEntryPrefab, buildingChoiceContainer);
            
            var createdButton = createdButtonGo.GetComponent<BuildingEntryUI>();
            
            createdButton.Initialize(building);
            createdButton.Selected += () => BuildingEntryOnSelected(createdButton, building);

            createdButtonGo.GetComponentInChildren<ToolTipSender>().TooltipDataProvider = building.GetTooltipData;
            
            return createdButton;
        }

        private void BuildingEntryOnSelected(BuildingEntryUI buildingEntry, Building building)
        {
            foreach (var buildingEntryUI in _buildingEntries)
                buildingEntryUI.ShowAsDeselected();
            
            _buildingController.SelectBuilding(building);
            buildingEntry.ShowAsSelected();
        }
    }
}