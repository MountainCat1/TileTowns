using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class OptionsUI : MonoBehaviour
    {
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider uiVolumeSlider;

        [Inject] private IGameSettingsAccessor _gameSettingsAccessor;

        private void OnEnable()
        {
            InitializeUIElements(_gameSettingsAccessor.Settings);
            
            sfxVolumeSlider.onValueChanged.AddListener(delegate { UpdateSettings(); });
            musicVolumeSlider.onValueChanged.AddListener(delegate { UpdateSettings(); });
            uiVolumeSlider.onValueChanged.AddListener(delegate { UpdateSettings(); });
        }
        
        private void OnDisable()
        {
            _gameSettingsAccessor.Save();
        }

        private void InitializeUIElements(GameSettings gameSettings)
        {
            sfxVolumeSlider.value = gameSettings.sfxVolume;
            musicVolumeSlider.value = gameSettings.muiscVolume;
            uiVolumeSlider.value = gameSettings.uiVolume;
        }

        public void UpdateSettings()
        {
            // Apply changes to _gameSettingsAccessor.Settings
            _gameSettingsAccessor.Settings.sfxVolume = sfxVolumeSlider.value;
            _gameSettingsAccessor.Settings.muiscVolume = musicVolumeSlider.value;
            _gameSettingsAccessor.Settings.uiVolume = uiVolumeSlider.value;

            _gameSettingsAccessor.Update();
        }
    }
}