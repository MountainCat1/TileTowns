using Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class BuildingPanelUI : MonoBehaviour
    {
        [SerializeField] private BuildingController buildingController;
        [SerializeField] private LevelManager levelManager;

        [SerializeField] private Transform buildingChoiceContainer;
        
        [SerializeField] private BuildingButtonUI buildingButtonPrefab;

        private void OnEnable()
        {
            levelManager.LevelLoaded += LevelManagerOnLevelLoaded;
        }

        private void LevelManagerOnLevelLoaded()
        {
            LoadBuildingData();
        }

        private void LoadBuildingData()
        {
            foreach (var buildingData in levelManager.LevelConfig.BuildingSet)
            {
                CreateBuildingButton(buildingData);
            }   
        }

        private void CreateBuildingButton(BuildingData buildingData)
        {
            var createdButton = Instantiate(buildingButtonPrefab, buildingChoiceContainer);
            
            createdButton.Initialize(buildingData);
        }
    }
}
