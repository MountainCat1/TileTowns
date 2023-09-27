using UnityEngine;

[CreateAssetMenu(menuName = "Data/Win Conditions/Gather Money")]
public class GatherMoneyWinCondition : WinCondition
{
    [field: SerializeField] public float Goal { get; set; }
    [field: SerializeField] public float TurnLimit { get; set; }

    public override GameResult Check(IGameState gameState)
    {
        var result = new GameResult
        {
            Progress = (gameState.Money / Goal) / 100f
        };

        if (result.Progress > 1)
            result.Progress = 1;
        
        if (gameState.Money >= Goal)
            return result.Win();

        if (gameState.Turn > TurnLimit - 1) // we subtract 1 bcs check happens at the end of the turn which means
                                            // otherwise it will always be late by one round
            return result.Lose();

        return result.Continue();
    }
}