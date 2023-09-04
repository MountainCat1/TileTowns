

public interface IGameStateTurnMutation : IGameStateMutation
{
    /// <summary>
    /// Object that imposed a change
    /// </summary>
    public object Mutator { get; }

    public decimal BuildingIncome { get; set; }
}

/// <summary>
/// Mutation that is applied every "turn"
/// </summary>
public class GameStateTurnMutation :  IGameStateTurnMutation
{
    /// <summary>
    /// Object that imposed a change
    /// </summary>
    public object Mutator { get; }

    public decimal? MoneyChange => BuildingIncome;

    public decimal BuildingIncome { get; set; }

    public GameStateTurnMutation(object mutator)
    {
        Mutator = mutator;
    }
}