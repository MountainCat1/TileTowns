using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ImmigrationPanelUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI changeDisplay;
        [SerializeField] private TextMeshProUGUI progressTextDisplay;
        [SerializeField] private Slider progressSliderDisplay;
        [SerializeField] private ToolTipSender toolTipSender;

        [SerializeField] private Color positiveColor;
        [SerializeField] private Color negativeColor;

        [Inject] private IGameState _gameState;

        [Inject]
        void Construct()
        {
            _gameState.Changed += UpdateData;
            _gameState.MutationChanged += UpdateMutationData;
        }

        private void Start()
        {
            toolTipSender.TooltipDataProvider = () => new TooltipData()
            {
                Title = "Immigration",
                Content =
                    $"Immigration: {_gameState.Immigration}\nImmigration change: {_gameState.ImmigrationChange}"
            };
        }

        private void UpdateMutationData()
        {
            var immigrationChange = Mathf.Round(_gameState.ImmigrationChange * 10) / 10;

            if (immigrationChange < 0)
                changeDisplay.text = $"{immigrationChange}%";
            else
                changeDisplay.text = $"+{immigrationChange}%";
            
            changeDisplay.color = immigrationChange >= 0 ? positiveColor : negativeColor;
        }

        private void UpdateData()
        {
            progressSliderDisplay.value = _gameState.Immigration;
            progressTextDisplay.text = $"{_gameState.Immigration}%";
        }
    }
}