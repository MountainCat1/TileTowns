using System;
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

        private Building _building;

        private void OnEnable()
        {
            button.onClick.AddListener(OnClicked);
        }
        
        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClicked);
        }

        public void Initialize(Building building)
        {
            buildingNameDisplay.text = building.name;
            priceDisplay.text = $"{building.Price}$";

            _building = building;
            
            if(building.Tile is not null)
                buildingImage.sprite = building.Tile.sprite;
            
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