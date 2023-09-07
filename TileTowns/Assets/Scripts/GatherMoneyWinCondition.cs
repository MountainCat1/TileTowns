public class GatherMoneyWinCondition : WinCondition
{
    public float Goal { get; set; }
    
    public override WinConditionCheckResult Check(GameState gameState)
    {
        if (gameState.Money >= Goal)
        {
            return Won;
        }

        return Continue;
    }
}