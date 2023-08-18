public class GatherMoneyWinCondition : WinCondition
{
    public decimal Goal { get; set; }
    
    public override WinConditionCheckResult Check(GameState gameState)
    {
        if (gameState.Money >= Goal)
        {
            return Won;
        }

        return Continue;
    }
}