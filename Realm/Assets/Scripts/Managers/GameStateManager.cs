using Zenject;

public interface IGameStateManager
{
}

public class GameStateManager : IGameStateManager
{
    [Inject] private IGameState _gameState;
    [Inject] private ITurnManager _turnManager;


    [Inject]
    private void Construct()
    {
        _turnManager.TurnEnded += OnTurnEnded;
        _turnManager.TurnStarted += OnTurnStarted;
        _turnManager.MutationHandlerRegistered += OnMutationHandlerRegistered;
    }

    private void OnMutationHandlerRegistered(ITurnMutationHandler turnMutationHandler)
    {
        turnMutationHandler.MutationChanged += () =>
        {
            RefreshMutator(turnMutationHandler);
        };
    }
    
    private void OnTurnStarted()
    {
        RefreshAllMutators();
    }

    private void OnTurnEnded()
    {
        _gameState.ApplyChanges();
    }

    private void RefreshAllMutators()
    {
        foreach (var mutator in _turnManager.MutationHandlers)
        {
            RefreshMutator(mutator);
        }
    }
    private void RefreshMutator(ITurnMutationHandler turnMutationHandler)
    {
        var mutation = turnMutationHandler.HandleTurn();
        
        _gameState.SetMutation(mutation);
    }
}