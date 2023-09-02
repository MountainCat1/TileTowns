using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingEntryUI : MonoBehaviour
    {
        public event Action Selected;
        
        [SerializeField] private TextMeshProUGUI buildingNameDisplay; 
        [SerializeField] private TextMeshProUGUI priceDisplay; 
        [SerializeField] private Image buildingImage;
        [SerializeField] private GameObject selection;
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
            buildingImage.sprite = buildingData.Tile.sprite;
            
            ShowAsDeselected();
        }

        public void OnClicked()
        {
            Selected?.Invoke();
        }

        public void ShowAsSelected()
        {
            selection.SetActive(true);
        }

        public void ShowAsDeselected()
        {
            selection.SetActive(false);
        }
    }
}