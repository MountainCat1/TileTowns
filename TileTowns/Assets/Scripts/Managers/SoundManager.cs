using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public interface ISoundManager
{
}

public class SoundManager : ISoundManager
{
    private const float DelayToDestroyNonPlayingAudioSource = 0.5f;
    
    [Inject] private IGameSounds _gameSounds;
    
    [Inject] private IBuildingController _buildingController;
    [Inject] private ITurnManager _turnManager;
    [Inject] private IGameManager _gameManager;
    
    [Inject] private Camera _camera;

    private Transform soundParent;

    [Inject]
    private void Construct()
    {
        soundParent = _camera.transform;
        
        _buildingController.PlaceBuildingFailed += delegate { PlaySound(_gameSounds.ErrorSound); };
        _buildingController.PlacedBuilding += delegate { PlaySound(_gameSounds.ErrorSound); };
        
        _turnManager.TurnEnded += delegate { PlaySound(_gameSounds.TurnEnded); };
        
        _gameManager.LevelLoaded += delegate { PlaySound(_gameSounds.GameMusic); };
    }

    private void PlaySound(AudioClip clip)
    {
        PlayAtPoint(clip, soundParent);
    }

    public static AudioSource PlayAtPoint(AudioClip clip, Transform parent, float volume = 1f)
    {
        GameObject audioObject = new GameObject("AudioPlayer");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        
        if (parent != null)
        {
            audioObject.transform.SetParent(parent);
        }

        audioSource.clip = clip;
        audioSource.Play();

        Object.Destroy(audioObject, clip.length + DelayToDestroyNonPlayingAudioSource);

        return audioSource;
    }
}