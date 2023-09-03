using Data;
using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {
        public override void OnTurn(Vector3Int position, GameStateChange change)
        {
            change.Income = CalculateIncome();
        }

        private decimal CalculateIncome()
        {
            return new decimal(5.0);
        }
    }
}