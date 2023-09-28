using System;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class ImmigrationManager : MonoBehaviour, IMutator
    {
        public event Action MutationChanged;
        
        [Inject] private IGameState _gameState;
        [Inject] private IGameConfig _gameConfig;
        [Inject] private ITurnManager _turnManager;

        [Inject]
        private void Construct()
        {
            _turnManager.RegisterMutator(this);
            _turnManager.TurnStarted += () => MutationChanged?.Invoke();
            _turnManager.TurnEnded += () => MutationChanged?.Invoke();
        }
        
        public IGameStateTurnMutation GetMutation()
        {
            return new GameStateTurnMutation(this)
            {
                ImmigrationChange = CalculateImmigrationChange()
            };
        }
        public IPersistentModifier GetPersistentModifier()
        {
            return new PersistentModifier()
            {
            };
        }
        
        private float CalculateImmigrationChange()
        {
            var immigrationConfig = _gameConfig.ImmigrationSettings;
            float immigrationDelta = 0f;

            immigrationDelta += immigrationConfig.ImmigrationPerPopulation * _gameState.Population;
            immigrationDelta += immigrationConfig.ImmigrationPerFreeHousing * (_gameState.Housing - _gameState.Population);
            immigrationDelta += immigrationConfig.ImmigrationPerFreeJob * (_gameState.WorkSlots - _gameState.Population);

            Debug.Log($"Immigration manager calculated immigration change: {immigrationDelta}");
            
            return immigrationDelta;
        }
    }
}