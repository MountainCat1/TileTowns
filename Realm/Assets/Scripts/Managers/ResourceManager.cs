using UnityEngine;
using Zenject;

public interface IResourceManager
{
    public bool SpendMoney(decimal amount);
}

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [Inject] private IGameState _gameState;

    public bool SpendMoney(decimal amount)
    {
        if (_gameState.Money < amount)
            return false;


        var mutation = new GameStateMutation
        {
            MoneyChange = amount
        };

        _gameState.ApplyMutation(mutation);

        return true;
    }
}