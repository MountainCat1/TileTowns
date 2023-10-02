using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    public class ButtonSoundUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [Inject] private IGameSounds _gameSounds;
        [Inject] private ISoundPlayer _soundPlayer;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _soundPlayer.PlaySound(_gameSounds.ButtonHover, SoundType.UI);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _soundPlayer.PlaySound(_gameSounds.ButtonClick, SoundType.UI);
        }
    }
}