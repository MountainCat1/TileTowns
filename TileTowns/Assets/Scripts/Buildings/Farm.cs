using Zenject;

namespace Buildings
{
    public class Farm : Building
    {
        [Inject] private ITileMapData _mapData;
        
        public override int WorkSlots => 4;
        public override int Housing => WorkSlots / 2;

        public float moneyPerWorker;

        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.BuildingIncome = moneyPerWorker * tileData.WorkersAssigned;
        }
    }
}