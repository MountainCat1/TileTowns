using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    public class LevelSet : ScriptableObject
    {
        [field: SerializeField] private List<LevelConfig> LevelConfigs;
    }
}