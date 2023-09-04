public interface IGameStateMutation
{
    decimal? MoneyChange { get; }
}

/// <summary>
/// Mutation to be applied immediately 
/// </summary>
public class GameStateMutation : IGameStateMutation
{
    public decimal? MoneyChange { get; set; }
}