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
            IBuildingController buildingController
        )
        {
            buildingController.PlaceBuildingFailed += () => LogError("Can't place building"); 
        }


        public void LogError(string message)
        {
            ErrorEntryUI entry = Instantiate(errorEntryPrefab, errorEntryContainer);

            entry.SetError(message);
            
            Destroy(entry.gameObject, timeToCleanEntry);
        }
    }
}