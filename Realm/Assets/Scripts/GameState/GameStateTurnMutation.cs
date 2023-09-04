

public interface IGameStateTurnMutation : IGameStateMutation
{
    /// <summary>
    /// Object that imposed a change
    /// </summary>
    public object Mutator { get; }
}

public class GameStateTurnMutation :  IGameStateTurnMutation
{
    /// <summary>
    /// Object that imposed a change
    /// </summary>
    public object Mutator { get; }

    public decimal? MoneyChange => BuildingIncome;

    public decimal BuildingIncome { get; set; }

    protected GameStateTurnMutation(object mutator)
    {
        Mutator = mutator;
    }

    public static GameStateTurnMutation New(object mutator)
    {
        return new GameStateTurnMutation(mutator);
    }
}