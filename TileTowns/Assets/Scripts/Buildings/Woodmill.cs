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
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            int adjacentForests = CountAdjacentForestTiles(tileData);
            
            float income = adjacentForests * MoneyPerAdjectedForest * MoneyPerWorker * tileData.WorkersAssigned;

            mutation.BuildingIncome = income;
        }

        private int CountAdjacentForestTiles(ITileData tileData)
        {
            var adjacentTiles = GetAdjacentTiles(tileData.Position);
            int amountOfAdjacentForests = adjacentTiles.Count(tile => tile.Feature == TileFeature.Forest);

            return amountOfAdjacentForests;
        }
    }
}