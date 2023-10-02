using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller<MainMenuInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
        
        Container.Bind<ILevelManager>().To<LevelManager>().FromComponentInHierarchy().AsSingle();
        
        Container.Bind<ISoundPlayer>().To<SoundPlayer>().AsSingle().NonLazy();
    }
}