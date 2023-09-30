using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {
        public override int WorkSlots => 4;
        public override int Housing => WorkSlots / 2;


        [field: SerializeField] public float MoneyPerWorker { get; set; }
        [field: SerializeField] public float ModifierPerAdjectedFarm { get; set; }

        [TextArea]
        [field: SerializeField] private string description =
            @"Farm is a building that produces income based on an amount of adjected farms.";

        public override string GetDescription()
        {
            return description;
        }

        public override void OnPlaced(ITileData tileData)
        {
            foreach (var adjacentTile in GetAdjacentTiles(tileData.Position))
            {
                adjacentTile.UpdateMutation();
            }
        }

        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            var adjectedTiles = GetAdjacentTiles(tileData.Position);
            
            var modifier = 1 + adjectedTiles.Count(x => x.Building is Farm) * ModifierPerAdjectedFarm;
            
            mutation.BuildingIncome = MoneyPerWorker * tileData.WorkersAssigned * modifier;
        }
    }
}