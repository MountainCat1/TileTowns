using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        
        Container.Bind<ILevelManager>().To<LevelManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IInputManager>().To<InputManager>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<IGameSettingsAccessor>().To<GameSettingsAccessor>().AsSingle().NonLazy();
        Container.Bind<IGameProgressAccessor>().To<GameProgressAccessor>().AsSingle().NonLazy();
        Container.Bind<ISoundPlayer>().To<SoundPlayer>().AsSingle().NonLazy();
    }
}