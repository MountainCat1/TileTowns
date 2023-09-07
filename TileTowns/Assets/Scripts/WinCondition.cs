public abstract class WinCondition
{
    public abstract WinConditionCheckResult Check(GameState gameState);

    public WinConditionCheckResult Won => new WinConditionCheckResult()
    {
        Won = true,
        Lost = false
    };
    
    public WinConditionCheckResult Lost => new WinConditionCheckResult()
    {
        Won = false,
        Lost = true
    };
    
    public WinConditionCheckResult Continue => new WinConditionCheckResult()
    {
        Won = false,
        Lost = false
    };
}

public class WinConditionCheckResult
{
    public bool Won { get; set; }
    public bool Lost { get; set; }
}