using Data;
using UnityEngine;

public interface IGameConfig
{
    ImmigrationSettings ImmigrationSettings { get; }
}

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/Game Config", order = 0)]
public class GameConfig : ScriptableObject, IGameConfig
{
    [field: SerializeField] public ImmigrationSettings ImmigrationSettings { get; set; }
}