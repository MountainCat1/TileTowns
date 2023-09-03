using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {
        public override void UpdateState(Vector3Int position, GameStateChange change)
        {
            change.BuildingIncome = CalculateIncome();
        }

        private decimal CalculateIncome()
        {
            return new decimal(5.0);
        }
    }
}