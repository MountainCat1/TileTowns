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
        [Inject] private IGameManager _gameManager;
        
        [Inject]
        private void Construct()
        {
            _turnManager.RegisterMutator(this);
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

            immigrationDelta += immigrationConfig.ImmigrationForPopulation * _gameState.Population;
            immigrationDelta += immigrationConfig.ImmigrationForFreeHousing * (_gameState.Housing - _gameState.Population);
            immigrationDelta += immigrationConfig.ImmigrationForFreeJob * (_gameState.WorkSlots - _gameState.Population);

            Debug.Log($"Immigration manager calculated immigration change: {immigrationDelta}");
            
            return immigrationDelta;
        }
    }
}