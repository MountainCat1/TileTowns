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
            if (_gameState.Money >= amount)
            {
                  var mutation = new GameStateMutation();

                  mutation.MoneyChange = amount;
                  
                  _gameState.ForceApplyMutation(mutation);

                  return true;
            }

            return false;
      }
}