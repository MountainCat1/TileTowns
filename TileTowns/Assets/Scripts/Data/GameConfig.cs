using UnityEngine;

public interface IGameConfig
{
    public float ImmigrationPerPopulation { get; set; }
}

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/Game Config", order = 0)]
public class GameConfig : ScriptableObject, IGameConfig
{
    [field: SerializeField] public float ImmigrationPerPopulation { get; set; } = 100;
    [field: SerializeField] public float ImmigrationForFreeHousing { get; set; } = 10;
    [field: SerializeField] public float ImmigrationForFreeWorkslots { get; set; } = 10;
}