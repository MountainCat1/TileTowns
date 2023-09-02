using Data;
using UnityEngine;
using Zenject;

namespace UI
{
    public class BuildingPanelUI : MonoBehaviour
    {
        [Inject] private IBuildingController _buildingController;
        [Inject] private LevelManager _levelManager;

        [SerializeField] private Transform buildingChoiceContainer;

        [SerializeField] private BuildingButtonUI buildingButtonPrefab;

        private void OnEnable()
        {
            _levelManager.LevelLoaded += LevelManagerOnLevelLoaded;
        }

        private void OnDisable()
        {
            _levelManager.LevelLoaded -= LevelManagerOnLevelLoaded;
        }


        private void LevelManagerOnLevelLoaded()
        {
            LoadBuildingData();
        }

        private void LoadBuildingData()
        {
            foreach (var buildingData in _levelManager.LevelConfig.BuildingSet)
            {
                CreateBuildingButton(buildingData);
            }
        }

        private void CreateBuildingButton(BuildingData buildingData)
        {
            var createdButton = Instantiate(buildingButtonPrefab, buildingChoiceContainer);

            createdButton.Initialize(buildingData);
            createdButton.Selected += CreatedButtonOnSelected;
        }

        private void CreatedButtonOnSelected(BuildingData buildingData)
        {
            _buildingController.SelectBuilding(buildingData.BuildingPrefab);
        }
    }
}