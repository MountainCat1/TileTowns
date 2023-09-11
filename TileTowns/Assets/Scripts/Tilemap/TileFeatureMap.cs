using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileFeature
{
    None,
    Forest,
    Mountain,
    Sea,
}


public interface ITileFeatureMap
{
    TileFeature GetMapping(TileBase tileBase);
}

[CreateAssetMenu(menuName = "TileFeatureMap")]
public class TileFeatureMap : ScriptableObject, ITileFeatureMap
{
    [field: SerializeField] public List<TileFeatureMapMapping> Mappings { get; set; }
    
    public TileFeature GetMapping(TileBase tileBase)
    {
        var mapping = Mappings.FirstOrDefault(mapping => mapping.Tile == tileBase);

        if (mapping is null)
            return TileFeature.None;

        return mapping.Feature;
    }
}

[System.Serializable]
public class TileFeatureMapMapping
{
    public TileBase Tile;
    public TileFeature Feature;
}