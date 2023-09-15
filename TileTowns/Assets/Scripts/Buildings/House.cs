using Zenject;

namespace Buildings
{
    public class House : Building
    {
        [Inject] private ITileMapData _mapData;
        
        public override int WorkSlots => 0;
        public override int Housing => 8;
        public override int Amenities => Housing / 2;

        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.ImmigrationChange = Amenities;
        }
    }
}