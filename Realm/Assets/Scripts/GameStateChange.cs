public class GameStateChange 
{
    /// <summary>
    /// Object that imposed a change
    /// </summary>
    public object Mutator { get; }
    
    public decimal BuildingIncome { get; set; }

    private GameStateChange(object mutator)
    {
        Mutator = mutator;
    }
    
    public static GameStateChange New(object mutator)
    {
        return new GameStateChange(mutator);
    }
}