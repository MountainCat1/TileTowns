using System.Linq;
using Zenject;

namespace Buildings
{
    public class Woodmill : Building
    {
        [Inject] private ITileMapData _mapData;
        
        public override int WorkSlots => 4;
        public override int Housing => 0;

        public float moneyPerWorker;
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            int adjacentForests = CountAdjacentForestTiles(tileData);
            mutation.BuildingIncome = moneyPerWorker * tileData.WorkersAssigned * adjacentForests;
        }

        private int CountAdjacentForestTiles(ITileData tileData)
        {
            var adjacentTiles = GetAdjacentTiles(tileData.Position);
            int amountOfAdjacentForests = adjacentTiles.Count(tile => tile.Feature == TileFeature.Forest);

            return amountOfAdjacentForests;
        }
    }
}