using UnityEngine;

namespace Data.WinConditions
{
    [CreateAssetMenu(menuName = "Data/Win Conditions/Gather Money")]
    public class GatherMoneyWinCondition : WinCondition
    {
        [field: SerializeField] public float Goal { get; set; }
        [field: SerializeField] public int TurnLimit { get; set; }

        public override string WinDescription => $"Accumulate {Goal}$ money";
        public override string LoseDescription => $"Don't take more than {TurnLimit} turns";

        public override IGameResult Check(IGameState gameState)
        {
            var result = new GameResult
            {
                WinProgress = (gameState.Money / Goal),
                LoseProgress = (gameState.Turn) / (float)TurnLimit
            };

            if (result.WinProgress > 1)
                result.WinProgress = 1;

            if (gameState.Money >= Goal)
                return result.Win();

            if (gameState.Turn > TurnLimit - 1) // we subtract 1 bcs check happens at the end of the turn which means
                // otherwise it will always be late by one round
                return result.Lose();

            return result.Continue();
        }
    }
}