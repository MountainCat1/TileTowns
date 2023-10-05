using UnityEngine;
using Zenject;

namespace Buildings
{
    public class House : Building
    {
        [Inject] private ITileMapData _mapData;

        [field: SerializeField] private int housingProvided;
        
        public override int WorkSlots => 0;
        public override int Housing => housingProvided;
        public override int ImmigrationChange => Housing / 2;

        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.ImmigrationChange = ImmigrationChange;
        }
    }
}