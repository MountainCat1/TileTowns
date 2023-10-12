using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class Church : Building
    {
        public override int WorkSlots => 2;
        public override int Housing => 0;
        public override int ImmigrationChange => 5;
        
        [field: SerializeField] public float MoneyPerHouse { get; set; }
        
        [TextArea]
        [field: SerializeField] private string description =
            @"Church provides $1 income per adjected house and some immigration.";

        public override string GetDescription()
        {
            return description.Replace("$1", MoneyPerHouse.ToString("0.00") + "$");
        }
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            var adjectedTiles = CountAdjacentHouseTiles(tileData);
            
            mutation.ImmigrationChange = ImmigrationChange * (1 / 1 + WorkSlots - tileData.WorkersAssigned);
            
            mutation.BuildingIncome = adjectedTiles * tileData.WorkersAssigned * MoneyPerHouse;
        }
        
        private int CountAdjacentHouseTiles(ITileData tileData)
        {
            var adjacentTiles = GetAdjacentTiles(tileData.Position);
            int houseCount = adjacentTiles.Count(tile => tile.Building is House);

            return houseCount;
        }
    }
}