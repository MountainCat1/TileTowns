using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {
        public override int WorkSlots => 4;
        public override int Housing => 0;

        [field: SerializeField] public float MoneyPerWorker { get; set; }
        
        public override void UpdateMutation(ITileData tileData, IGameStateTurnMutation mutation)
        {
            mutation.BuildingIncome = MoneyPerWorker * tileData.WorkersAssigned;
            mutation.ImmigrationChange = MoneyPerWorker * tileData.WorkersAssigned;
        }
    }
}