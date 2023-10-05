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

        [Inject] private IBuildingController _buildingController;
        [Inject] private IPopulationController _populationController;
        [Inject] private IGameManager _gameManager;
        
        [Inject]
        private void Construct()
        {
            _turnManager.RegisterMutator(this);

            _gameManager.LevelLoaded += UpdateMutation;
            
            // TODO: This is a hack, we need to find a better way to update the mutation
            _buildingController.PlacedBuilding += (_, _) => UpdateMutation();
            _turnManager.TurnEnded += UpdateMutation;
            _turnManager.TurnStarted += UpdateMutation;
            _populationController.WorkerAssigned += (_) => UpdateMutation();
            _populationController.WorkerUnassigned += (_) => UpdateMutation();
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

        private void UpdateMutation()
        {
            MutationChanged?.Invoke();
        }
    }
}