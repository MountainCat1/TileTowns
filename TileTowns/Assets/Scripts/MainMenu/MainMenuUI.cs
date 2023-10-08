using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [Inject] private IInputManager _inputManager;
        
        [SerializeField] private MenuViewUI defaultView;
        [SerializeField] private List<MenuViewUI> views;
        
        private MenuViewUI _currentView;
        private Stack<MenuViewUI> _viewStack = new Stack<MenuViewUI>();

        private void Start()
        {
            _inputManager.PlayerPressedBack += GoBack;
            
            
            ShowPage(defaultView, false);
        }

        public void ShowPage(MenuViewUI targetView, bool addToStack)
        {
            if(addToStack)
                _viewStack.Push(_currentView);
            
            foreach (var page in views)
            {
                page.gameObject.SetActive(false);
            }
            
            targetView.gameObject.SetActive(true);
            _currentView = targetView;
        }
        
        public void ShowPage(MenuViewUI targetView)
        {
            ShowPage(targetView, true);
        }

        private void GoBack()
        {
            if (!_viewStack.Any())
            {
                ShowPage(defaultView, false);
                return;
            }
            
            var previousPage = _viewStack.Pop();
            ShowPage(previousPage, false);
        }
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}