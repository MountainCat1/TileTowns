using UnityEngine;

public abstract class WinCondition : ScriptableObject
{
    public abstract GameResult Check(IGameState gameState);

    public GameResult Won => new GameResult()
    {
        Won = true,
        Lost = false
    };
    
    public GameResult Lost => new GameResult()
    {
        Won = false,
        Lost = true
    };
    
    public GameResult Continue => new GameResult()
    {
        Won = false,
        Lost = false
    };
}

public class GameResult
{
    public bool Won { get; set; }
    public bool Lost { get; set; }
}