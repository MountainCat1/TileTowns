using System.Collections.Generic;
using Data;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BuildingPanelUI : MonoBehaviour
    {
        [Inject] private IBuildingController _buildingController;
        [Inject] private IGameManager _gameManager;

        [SerializeField] private Transform buildingChoiceContainer;

        [SerializeField] private BuildingEntryUI buildingEntryPrefab;

        private List<BuildingEntryUI> _buildingEntries; 

        private void OnEnable()
        {
            _gameManager.LevelLoaded += GameManagerOnGameLoaded;
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

        private BuildingEntryUI CreateBuildingEntries(BuildingData buildingData)
        {
            var createdButton = Instantiate(buildingEntryPrefab, buildingChoiceContainer);

            createdButton.Initialize(buildingData);
            createdButton.Selected += () => CreatedButtonOnSelected(createdButton, buildingData);
            
            return createdButton;
        }

        private void CreatedButtonOnSelected(BuildingEntryUI buildingEntry, BuildingData buildingData)
        {
            foreach (var buildingEntryUI in _buildingEntries)
                buildingEntryUI.ShowAsDeselected();
            
            _buildingController.SelectBuilding(buildingData);
            buildingEntry.ShowAsSelected();
        }
    }
}