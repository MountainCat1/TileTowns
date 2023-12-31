﻿using UnityEngine;

namespace Data.WinConditions
{
    [CreateAssetMenu(menuName = "Data/Win Conditions/Get Population")]
    public class GetPopulationWinConditions : WinCondition
    {
        [field: SerializeField] public float Goal { get; set; }
        [field: SerializeField] public int TurnLimit { get; set; }

        public override string WinDescription => $"Reach {Goal} population";
        public override string LoseDescription => $"Don't take more than {TurnLimit} turns";

        public override IGameResult Check(IGameState gameState)
        {
            var result = new GameResult
            {
                WinProgress = (gameState.Population / Goal),
                LoseProgress = (gameState.Turn) / (float)TurnLimit
            };

            if (result.WinProgress > 1)
                result.WinProgress = 1;

            if (gameState.Population >= Goal)
                return result.Win();

            if (gameState.Turn > TurnLimit - 1) // we subtract 1 bcs check happens at the end of the turn which means
                // otherwise it will always be late by one round
                return result.Lose();

            return result.Continue();
        }
    }
}