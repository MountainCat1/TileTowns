using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TilePopulationUI : MonoBehaviour
    {
        [SerializeField] private Color assignedPopColor;
        [SerializeField] private Color unassignedPopColor;
        [SerializeField] private GameObject popDisplayPrefab;
        [SerializeField] private Transform popDisplayContainer;
        
        private TileData _tileData;
        
        public void Initialize(TileData tileData)
        {
            _tileData = tileData;
            
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            foreach (Transform containerChild in popDisplayContainer)
            {
                Destroy(containerChild.gameObject);
            }

            for (int i = 0; i < _tileData.WorkersAssigned; i++)
            {
                var go = Instantiate(popDisplayPrefab, popDisplayContainer);

                go.GetComponent<Image>().color = assignedPopColor;
            }

            if (_tileData.Building is null)
                throw new InvalidOperationException();
            
            for (int i = 0; i < _tileData.Building.WorkSlots - _tileData.WorkersAssigned; i++)
            {
                var go = Instantiate(popDisplayPrefab, popDisplayContainer);
                    
                go.GetComponent<Image>().color = unassignedPopColor;
            }
        }
    }
}