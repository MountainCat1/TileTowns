using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public interface ILevelSet
    {
        List<LevelConfig> LevelConfigs { get; }
    }

    [CreateAssetMenu(menuName = "Data/Level Set")]
    public class LevelSet : ScriptableObject, ILevelSet
    {
        [field: SerializeField] public List<LevelConfig> LevelConfigs { get; private set; }
    }
}