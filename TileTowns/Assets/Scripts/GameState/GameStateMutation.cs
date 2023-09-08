public interface IGameStateMutation
{
    float? MoneyChange { get; }
    float? ImmigrationChange { get; set; }
    int? PopulationChange { get; set; }
}

/// <summary>
/// Mutation to be applied immediately 
/// </summary>
public class GameStateMutation : IGameStateMutation
{
    public float? MoneyChange { get; set; }
    public float? ImmigrationChange { get; set; }
    public int? PopulationChange { get; set; }
}