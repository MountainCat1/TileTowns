using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {
        public override int WorkSlots => workSlots;
        public override int Housing => housing;

        [field: SerializeField] public float MoneyPerWorker { get; set; }
        [field: SerializeField] public float ModifierPerAdjectedFarm { get; set; }
        
        [field: SerializeField] private int housing = 0;
        [field: SerializeField] private int workSlots = 4;

        [TextArea]
        [field: SerializeField] private string description =
            @"Farm allows to produces workes $1 income per adjected farm.";

        public override string GetDescription()
        {
            return description.Replace("$1", MoneyPerWorker.ToString("0.00") + "$");
        }
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            // Farm gets more income based on amount of adjected farms
            
            var adjectedTiles = GetAdjacentTiles(tileData.Position);
            
            var modifier = 1 + adjectedTiles.Count(x => x.Building is Farm) * ModifierPerAdjectedFarm;
            
            mutation.BuildingIncome = MoneyPerWorker * tileData.WorkersAssigned * modifier;
        }
    }
}