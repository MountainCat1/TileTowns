using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/Building Metadata", fileName = nameof(Building))]
    public class Building : TileBase
    {
        [field: SerializeField] public Tile Tile { get; set; }
        [field: SerializeField] public float Price { get; set; }

        public virtual void OnTurn(Vector3Int position, GameStateChange change)
        {
            
        }

        public virtual string GetDescription()
        {
            return "NO DESCRIPTION??? WTH?!!!";
        }
    }
}