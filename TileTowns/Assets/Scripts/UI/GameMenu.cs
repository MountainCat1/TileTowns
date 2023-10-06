using System;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameMenu : MonoBehaviour
    {
        [Inject] private IGameManager _gameManager;
        [Inject] private ILevelManager _levelManager;
        [Inject] private IInputManager _inputManager;

        [SerializeField] private Transform menu;

        private bool shown = false;

        private void OnEnable()
        {
            _inputManager.OnEscapePressed += OnEscapePressed;
        }

        private void OnEscapePressed()
        {
            if(_gameManager.GameStage is not (GameStage.Pause or GameStage.Playing))
                return;
            
            if (shown)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void Show()
        {
            menu.gameObject.SetActive(true);
            _gameManager.GameStage = GameStage.Pause;
            shown = true;
        }

        private void Hide()
        {                
            menu.gameObject.SetActive(false);
            _gameManager.GameStage = GameStage.Playing;
            shown = false;
        }
        
        public void Quit()
        {
            _levelManager.LoadMainMenu();
        }

        public void Retry()
        {
            _gameManager.Restart();
        }
    
        public void Continue()
        {
            Hide();
        }
    }
}