public interface IGameStateMutation
{
    /// <summary>
    /// Object that imposed a change
    /// </summary>
    object Mutator { get; }

    decimal BuildingIncome { get; set; }
}

public class GameStateMutation : GameStateData, IGameStateMutation
{
    /// <summary>
    /// Object that imposed a change
    /// </summary>
    public object Mutator { get; }
    
    public decimal BuildingIncome { get; set; }
    
    private GameStateMutation(object mutator)
    {
        Mutator = mutator;
    }
    
    public static GameStateMutation New(object mutator)
    {
        return new GameStateMutation(mutator);
    }
}