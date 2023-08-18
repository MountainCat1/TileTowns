using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelDescriptor : MonoBehaviour
{
    [field: SerializeField] public Tilemap Map { get; set; }
}
