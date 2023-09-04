using UnityEngine;
using Zenject;

public interface IResourceController
{
    bool SpendMoney(decimal amount);
}

public class ResourceController : MonoBehaviour, IResourceController
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