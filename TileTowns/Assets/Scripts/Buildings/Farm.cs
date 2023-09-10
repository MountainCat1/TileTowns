using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {
        public override int WorkSlots => 4;

        public override void UpdateMutation(Vector3Int position, IGameStateTurnMutation mutation)
        {
            mutation.BuildingIncome = CalculateIncome();
            mutation.ImmigrationChange = 5;
            mutation.Housing = 1;
        }

        private float CalculateIncome()
        {
            // TODO add farm income logic here pls 🐲
            return 5f;
        }
    }
}