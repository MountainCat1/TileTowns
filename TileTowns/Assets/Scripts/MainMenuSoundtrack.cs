using UnityEngine;
using Zenject;

public class MainMenuSoundtrack : MonoBehaviour
{
    [Inject] private ISoundPlayer _soundPlayer;
    [Inject] private IGameSounds _gameSounds;

    private void Start()
    {
        _soundPlayer.PlaySound(_gameSounds.MenuMusic);
    }
}
