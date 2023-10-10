﻿using System.Linq;
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
            @"Farm is a building that produces income based on an amount of adjected farms.";

        public override string GetDescription()
        {
            return description;
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