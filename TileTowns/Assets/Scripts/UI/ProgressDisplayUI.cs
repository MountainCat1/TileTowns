using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ProgressDisplayUI : MonoBehaviour
    {
        [SerializeField] private Slider winProgressSlider;
        [SerializeField] private Slider loseProgressSlider;
        
        [SerializeField] private TextMeshProUGUI winProgressText;
        [SerializeField] private TextMeshProUGUI loseProgressText;
        
        [SerializeField] private TextMeshProUGUI winConditionText;
        [SerializeField] private TextMeshProUGUI loseConditionText;

        [Inject] private IGameManager _gameManager;
        
        [Inject]
        private void Construct()
        {
            _gameManager.GameResultChanged += OnGameResultChanged;
            _gameManager.LevelLoaded += OnLevelLoaded;
        }

        private void OnLevelLoaded()
        {
            var winCondition = _gameManager.LevelConfig.WinCondition;

            winConditionText.text = winCondition.WinDescription;
            loseConditionText.text = winCondition.LoseDescription;
        }


        private void OnGameResultChanged(IGameResult gameResult)
        {
            winProgressSlider.value = gameResult.WinProgress;
            loseProgressSlider.value = gameResult.LoseProgress;

            winProgressText.text = $"{Mathf.Round(gameResult.WinProgress * 1000) / 1000 * 100}%";
            loseProgressText.text = $"{Mathf.Round(gameResult.LoseProgress * 1000) / 1000 * 100}%";
        }
    }
}