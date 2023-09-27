using UnityEngine;

public abstract class WinCondition : ScriptableObject
{
    public abstract IGameResult Check(IGameState gameState);
}

public interface IGameResult
{
    bool Won { get; set; }
    bool Lost { get; set; }
    
    float WinProgress { get; set; }
    float LoseProgress { get; set; }
    
    IGameResult Continue();
    IGameResult Win();
    IGameResult Lose();
}

public class GameResult : IGameResult
{
    public bool Won { get; set; }
    public bool Lost { get; set; }
    public float WinProgress { get; set; }
    public float LoseProgress { get; set; }

    public IGameResult Continue()
    {
        Won = false;
        Lost = false;
        
        return this;
    }

    public IGameResult Win()
    {
        Won = true;
        Lost = false;

        return this;
    }
    
    
    public IGameResult Lose()
    {
        Won = false;
        Lost = true;

        return this;
    }
}