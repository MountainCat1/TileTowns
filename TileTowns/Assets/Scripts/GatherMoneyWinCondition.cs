using UnityEngine;

[CreateAssetMenu(menuName = "Data/Win Conditions/Gather Money")]
public class GatherMoneyWinCondition : WinCondition
{
    [field: SerializeField] public float Goal { get; set; }

    public override WinConditionCheckResult Check(IGameState gameState)
    {
        if (gameState.Money >= Goal)
        {
            return Won;
        }

        return Continue;
    }
}