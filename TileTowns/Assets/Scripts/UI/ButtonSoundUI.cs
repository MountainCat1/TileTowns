using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class ButtonSoundUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [Inject] private IGameSounds _gameSounds;
        [Inject] private ISoundPlayer _soundPlayer;

        private void Start()
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _soundPlayer.PlaySound(_gameSounds.ButtonHover);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _soundPlayer.PlaySound(_gameSounds.ButtonClick);
        }
    }
}