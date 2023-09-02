using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingButtonUI : MonoBehaviour
    {
        public event Action<BuildingData> Selected;
        
        [SerializeField] private TextMeshProUGUI buildingNameDisplay; 
        [SerializeField] private TextMeshProUGUI priceDisplay; 
        [SerializeField] private Image buildingImage;
        [SerializeField] private Button button;

        private BuildingData _buildingData;

        private void OnEnable()
        {
            button.onClick.AddListener(OnClicked);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClicked);
        }

        public void Initialize(BuildingData buildingData)
        {
            buildingNameDisplay.text = buildingData.name;
            priceDisplay.text = $"{buildingData.Price}$";

            _buildingData = buildingData;
            // _buildingImage.sprite = buildingData
        }

        public void OnClicked()
        {
            Selected?.Invoke(_buildingData);
        }
    }
}