using Data;
using UnityEngine;
using Zenject;

namespace Buildings
{
    public class Farm : Building
    {
        public override void OnTurn(Vector3Int position)
        {
            base.OnTurn(position);
        }

        private decimal CalculateIncome()
        {
            return new decimal(5.0);
        }
    }
}