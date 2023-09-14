using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class PresentTurnDisplayUI : MonoBehaviour
    {
        [Inject] private ITurnManager _turnManager;

        [SerializeField] private TextMeshProUGUI turnDisplay;
        
        private void Start()
        {
            _turnManager.TurnStarted += () =>
            {
                turnDisplay.text = $"{_turnManager.TurnCount}";
            };
        }
    }
}