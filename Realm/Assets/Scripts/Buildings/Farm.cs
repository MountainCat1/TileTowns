using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {
        public override void CreateMutation(Vector3Int position, IGameStateTurnMutation mutation)
        {
            mutation.BuildingIncome = CalculateIncome();
        }

        private decimal CalculateIncome()
        {
            // TODO add farm income logic here pls 🐲
            return new decimal(5.0);
        }
    }
}