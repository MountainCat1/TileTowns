using UnityEngine;

public interface IGameSounds
{
    AudioClip ErrorSound { get; set; }
    AudioClip Building { get; set; }
    AudioClip TurnEnded { get; set; }
    AudioClip GameMusic { get; set; }
    AudioClip MenuMusic { get; set; }
}

[CreateAssetMenu(menuName = "Data/Game Sounds")]
public class GameSounds : ScriptableObject, IGameSounds
{
    [field: SerializeField] public AudioClip ErrorSound { get; set; }
    [field: SerializeField] public AudioClip Building { get; set; }
    [field: SerializeField] public AudioClip TurnEnded { get; set; }
    
    [field: SerializeField] public AudioClip GameMusic { get; set; }
    [field: SerializeField] public AudioClip MenuMusic { get; set; }
}