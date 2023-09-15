using UnityEngine;
using Zenject;

namespace Buildings
{
    public class Church : Building
    {
        [Inject] private ITileMapData _mapData;

        public override int WorkSlots => 2;
        public override int Housing => 2;

        [field: SerializeField] public float CostPerWorker { get; set; }

        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.BuildingIncome = -CostPerWorker * tileData.WorkersAssigned;
            mutation.ImmigrationChange = 50 * tileData.WorkersAssigned;
        }
    }
}