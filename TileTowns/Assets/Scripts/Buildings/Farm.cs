using UnityEngine;
using Zenject;

namespace Buildings
{
    public class Farm : Building
    {
        [Inject] private ITileMapData _mapData;
        
        public override int WorkSlots => 4;
        public override int Housing => WorkSlots / 2;

        [field: SerializeField] public float MoneyPerWorker { get; set; }
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.BuildingIncome = MoneyPerWorker * tileData.WorkersAssigned;
        }
    }
}