using Zenject;

namespace Buildings
{
    public class Church : Building
    {
        [Inject] private ITileMapData _mapData;

        public override int WorkSlots => 2;
        public override int Housing => 2;
        public override int Amenities => 10;

        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.ImmigrationChange = Amenities * (1 / 1 + WorkSlots - tileData.WorkersAssigned);
        }
    }
}