using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Data/Game Config Installer")]
public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
{
    [SerializeField] private GameConfig gameConfig;
    public override void InstallBindings()
    {
        Container.Bind<IGameConfig>().To<GameConfig>().FromInstance(gameConfig).AsSingle();
    }
}