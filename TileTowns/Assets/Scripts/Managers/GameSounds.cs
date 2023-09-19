using UnityEngine;
using UnityEngine.Serialization;

public interface IGameSounds
{
    AudioClip ErrorSound { get; set; }
    AudioClip Building { get; set; }
}

[CreateAssetMenu(menuName = "Data/Game Sounds")]
public class GameSounds : ScriptableObject, IGameSounds
{
    [field: SerializeField] public AudioClip ErrorSound { get; set; }
    [field: SerializeField] public AudioClip Building { get; set; }
}