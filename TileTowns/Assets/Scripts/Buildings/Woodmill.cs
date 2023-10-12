using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Buildings
{
    public class Woodmill : Building
    {
        [Inject] private ITileMapData _mapData;
        
        public override int WorkSlots => 4;
        public override int Housing => 0;

        [field: SerializeField] public float MoneyPerWorker { get; set; }
        [field: SerializeField] public float MoneyPerAdjectedForest { get; set; }
        
        [TextArea]
        [field: SerializeField] private string description =
            @"Woodmilll allows to produces workes $1 income per adjected forest.";

        public override string GetDescription()
        {
            return description.Replace("$1", MoneyPerAdjectedForest.ToString("0.00") + "$");
        }
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            int adjacentForests = CountAdjacentForestTiles(tileData);
            
            float baseIncome = adjacentForests * MoneyPerAdjectedForest * tileData.WorkersAssigned;
            float bonusIncome = MoneyPerWorker * tileData.WorkersAssigned;
            
            mutation.BuildingIncome = baseIncome + bonusIncome;
        }

        private int CountAdjacentForestTiles(ITileData tileData)
        {
            var adjacentTiles = GetAdjacentTiles(tileData.Position);
            int amountOfAdjacentForests = adjacentTiles.Count(tile => tile.Feature == TileFeature.Forest);

            return amountOfAdjacentForests;
        }
    }
}