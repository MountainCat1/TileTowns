using DefaultNamespace;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller<GameInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<ILevelManager>().To<LevelManager>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<IInputManager>().To<InputManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IGameState>().To<GameState>().AsSingle();
        Container.Bind<IResourceManager>().To<ResourceManager>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IGameManager>().To<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ITileMapData>().To<TileMapData>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ITileSelector>().To<TileSelector>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IBuildingController>().To<BuildingController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ITurnManager>().To<TurnManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IPlayerController>().To<PlayerController>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IToolTipController>().To<ToolTipController>().FromComponentsInHierarchy().AsSingle();
        Container.Bind<IPopulationController>().To<PopulationController>().FromComponentsInHierarchy().AsSingle();

        Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        
        Container.Bind<IGameSettingsAccessor>().To<GameSettingsAccessor>().AsSingle().NonLazy();
        Container.Bind<ISoundPlayer>().To<SoundPlayer>().AsSingle().NonLazy();
        Container.Bind<ISoundManager>().To<SoundManager>().AsSingle().NonLazy();
    }      
}