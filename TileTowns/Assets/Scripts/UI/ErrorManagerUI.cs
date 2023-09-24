using UnityEngine;
using Zenject;

namespace UI
{
    public class ErrorManagerUI : MonoBehaviour
    {
        [SerializeField] private ErrorEntryUI errorEntryPrefab;
        [SerializeField] private Transform errorEntryContainer;

        [SerializeField] private float timeToCleanEntry = 15f;

        [Inject]
        private void Construct(
            IBuildingController buildingController,
            IPopulationController populationController
        )
        {
            buildingController.PlaceBuildingFailed += () => LogError("Cannot place building"); 
            populationController.WorkerAssignedFailed += (_) => LogError("Cannot assign more workers"); 
        }


        public void LogError(string message)
        {
            ErrorEntryUI entry = Instantiate(errorEntryPrefab, errorEntryContainer);

            entry.SetError(message);
            
            Destroy(entry.gameObject, timeToCleanEntry);
        }
    }
}