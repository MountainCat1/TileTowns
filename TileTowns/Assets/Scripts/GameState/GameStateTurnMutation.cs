

public interface IGameStateTurnMutation : IGameStateMutation
{
    public float BuildingIncome { get; set; }
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

    public float? MoneyChange => BuildingIncome;
    public float? ImmigrationChange { get; set; }
    public int? PopulationChange { get; set; }

    public float BuildingIncome { get; set; }

    public GameStateTurnMutation(object mutator)
    {
        Mutator = mutator;
    }
}