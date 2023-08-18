using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Game Configuration")]
public class Level : ScriptableObject
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public LevelDescriptor LevelDescriptor { get; set; }
}
