public class GameStateChange : GameStateData
{
    /// <summary>
    /// Object that imposed a change
    /// </summary>
    public object Mutator { get; }
    
    private GameStateChange(object mutator)
    {
        Mutator = mutator;
    }
    
    public static GameStateChange New(object mutator)
    {
        return new GameStateChange(mutator);
    }
}