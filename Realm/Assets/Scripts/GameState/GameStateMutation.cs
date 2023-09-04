public interface IGameStateMutation
{
    decimal? MoneyChange { get; }
}

public class GameStateMutation : IGameStateMutation
{
    public decimal? MoneyChange { get; set; }
}