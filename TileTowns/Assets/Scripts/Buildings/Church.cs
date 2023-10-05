using System.Linq;
using UnityEngine;
using Zenject;

namespace Buildings
{
    public class Church : Building
    {
        [Inject] private ITileMapData _mapData;

        public override int WorkSlots => 2;
        public override int Housing => 2;
        public override int ImmigrationChange => 10;
        
        [field: SerializeField] public float MoneyPerHouse { get; set; }
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            var adjectedTiles = CountAdjacentHouseTiles(tileData);
            
            mutation.ImmigrationChange = ImmigrationChange * (1 / 1 + WorkSlots - tileData.WorkersAssigned);
            
            mutation.BuildingIncome = adjectedTiles * tileData.WorkersAssigned * MoneyPerHouse;
        }
        
        private int CountAdjacentHouseTiles(ITileData tileData)
        {
            var adjacentTiles = GetAdjacentTiles(tileData.Position);
            int amountOfAdjacentForests = adjacentTiles.Count(tile => tile.Building is House);

            return amountOfAdjacentForests;
        }
    }
}