using System;
using UnityEngine;
using Zenject;

public interface IPopulationMigration
{
    float HousingImmigration { get; }
    float WorkSlotsImmigration { get; }
    float PopulationImmigration { get; }
    float GetSum();
}

public class PopulationMigration : IPopulationMigration
{
    public float HousingImmigration { get; set; }
    public float WorkSlotsImmigration { get; set; }
    public float PopulationImmigration { get; set; }
    
    public float GetSum()
    {
        return HousingImmigration + WorkSlotsImmigration + PopulationImmigration;
    }
}

public class ImmigrationManager : MonoBehaviour, IMutator
{
    public event Action MutationChanged;
    public event Action<IPopulationMigration> ImmigrationChanged;

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

        var populationMigration = new PopulationMigration()
        {
            PopulationImmigration = ApplyModifier(immigrationConfig.ImmigrationForPopulation * _gameState.Population),
            WorkSlotsImmigration = ApplyModifier(immigrationConfig.ImmigrationForFreeJob *
                                                 (_gameState.WorkSlots - _gameState.Population)),
            HousingImmigration = ApplyModifier(immigrationConfig.ImmigrationForFreeHousing *
                                               (_gameState.Housing - _gameState.Population))
        };

        ImmigrationChanged?.Invoke(populationMigration);
        
        var immigrationDelta = populationMigration.GetSum();

        Debug.Log($"Immigration manager calculated immigration change: {immigrationDelta}");

        return immigrationDelta;
    }

    private float ApplyModifier(float f)
    {
        if (f >= 0)
            return f;
        else
            return f * _gameConfig.ImmigrationSettings.NegativeMultiplier;
    }

    private void UpdateMutation()
    {
        MutationChanged?.Invoke();
    }
}