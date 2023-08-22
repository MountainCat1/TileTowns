using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game Configuration")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public LevelDescriptor LevelDescriptor { get; set; }
}
