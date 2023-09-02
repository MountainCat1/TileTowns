public class GameState
{
    public decimal Money { get; set; }

    public void ApplyChange(GameStateChange change)
    {
        Money += change.Income;
    }
}