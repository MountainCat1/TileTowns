using UnityEngine;

public interface IGameSounds
{
    AudioClip Error { get; set; }
    AudioClip Building { get; set; }
    AudioClip TurnEnded { get; set; }
    AudioClip GameMusic { get; set; }
    AudioClip MenuMusic { get; set; }
    AudioClip WorkerAssigned { get; set; }
    AudioClip WorkerUndassigned { get; set; }
    AudioClip ButtonClick { get; set; }
    AudioClip ButtonHover { get; set; }
    AudioClip Lose { get; set; }
    AudioClip Win { get; set; }
    AudioClip LoseMusic { get; set; }
    AudioClip WinMusic { get; set; }
}

[CreateAssetMenu(menuName = "Data/Game Sounds")]
public class GameSounds : ScriptableObject, IGameSounds
{
    [field: SerializeField] public AudioClip Error { get; set; }
    [field: SerializeField] public AudioClip Building { get; set; }
    [field: SerializeField] public AudioClip TurnEnded { get; set; }
    
    [field: SerializeField] public AudioClip WorkerAssigned { get; set; }
    [field: SerializeField] public AudioClip WorkerUndassigned { get; set; }
    
    [field: SerializeField] public AudioClip GameMusic { get; set; }
    [field: SerializeField] public AudioClip MenuMusic { get; set; }
    
    [field: SerializeField] public AudioClip ButtonClick { get; set; }
    [field: SerializeField] public AudioClip ButtonHover { get; set; }
    
    [field: SerializeField] public AudioClip Lose { get; set; }
    [field: SerializeField] public AudioClip Win { get; set; }

    [field: SerializeField] public AudioClip LoseMusic { get; set; }
    [field: SerializeField] public AudioClip WinMusic { get; set; }
}