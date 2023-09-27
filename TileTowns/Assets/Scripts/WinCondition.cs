using UnityEngine;

public abstract class WinCondition : ScriptableObject
{
    public abstract GameResult Check(IGameState gameState);
}

public class GameResult
{
    public bool Won { get; set; }
    public bool Lost { get; set; }
    public float Progress { get; set; }

    public GameResult Continue()
    {
        Won = false;
        Lost = false;
        
        return this;
    }

    public GameResult Win()
    {
        Won = true;
        Lost = false;

        return this;
    }
    
    
    public GameResult Lose()
    {
        Won = false;
        Lost = true;

        return this;
    }
}