using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public interface ILevelSet
    {
        List<LevelConfig> LevelConfigs { get; }
    }

    public class LevelSet : ScriptableObject, ILevelSet
    {
        [field: SerializeField] public List<LevelConfig> LevelConfigs { get; private set; }
    }
}