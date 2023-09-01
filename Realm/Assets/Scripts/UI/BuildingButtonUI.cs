using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingButtonUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI buildingNameDisplay; 
        [SerializeField] private TextMeshProUGUI priceDisplay; 
        [SerializeField] private Image buildingImage; 
        
        public void Initialize(BuildingData buildingData)
        {
            buildingNameDisplay.text = buildingData.name;
            priceDisplay.text = $"{buildingData.Price}$";

            // _buildingImage.sprite = buildingData
        }
    }
}