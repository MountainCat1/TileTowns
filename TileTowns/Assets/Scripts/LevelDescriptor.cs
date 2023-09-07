using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelDescriptor : MonoBehaviour
{
    [field: SerializeField] public Tilemap Map { get; set; }
}
