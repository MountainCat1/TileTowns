using System.Collections.Generic;
using Data;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Level Config")]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public string LevelName { get; set; }

    [field: SerializeField]
    [field: TextArea]
    public string LevelDescription { get; set; }

    [field: SerializeField] public LevelDescriptor LevelDescriptor { get; set; }
    [field: SerializeField] public AudioClip Soundtrack { get; set; }
    [field: SerializeField] public BuildingMetadataSet BuildingSet { get; set; }
    [field: SerializeField] public Sprite Thumbnail { get; set; }


    [field: SerializeField] public int InitialPopulation { get; set; }
    [field: SerializeField] public float InitialMoney { get; set; }

    [field: SerializeField] public WinCondition WinCondition { get; set; }
    
    [field: SerializeField] public List<InitialBuilding> InitialBuildings { get; set; }
}

[System.Serializable]
public class InitialBuilding
{
    [SerializeField] public Vector2Int position;
    [SerializeField] public Building building;
}