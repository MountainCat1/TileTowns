public interface IGameState
{
    decimal Money { get; set; }
}

public class GameState : IGameState
{
    public decimal Money { get; set; }
    public GameStateChange GameStateChange { get; set; }
    
    public GameState(ITurnManager turnManager)
    {
        GameStateChange = new GameStateChange();
        
        turnManager.TurnCalculated += TurnManagerOnTurnCalculated;
    }

    private void TurnManagerOnTurnCalculated()
    {
        ApplyChange(GameStateChange);

        GameStateChange = new GameStateChange();
    }


    private void ApplyChange(GameStateChange change)
    {
        Money += change.Income;
    }
}