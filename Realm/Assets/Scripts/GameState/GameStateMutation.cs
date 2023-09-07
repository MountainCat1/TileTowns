public interface IGameStateMutation
{
    float? MoneyChange { get; }
}

/// <summary>
/// Mutation to be applied immediately 
/// </summary>
public class GameStateMutation : IGameStateMutation
{
    public float? MoneyChange { get; set; }
}