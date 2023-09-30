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

        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            var modifier = 1 + GetAdjacentTiles(tileData.Position)
                .Count(x => x.Building is Farm) 
                * ModifierPerAdjectedFarm;
            
            mutation.BuildingIncome = MoneyPerWorker * tileData.WorkersAssigned * modifier;
        }
    }
}